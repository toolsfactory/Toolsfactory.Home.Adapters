using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.CommandLine;
using Toolsfactory.Home.Adapters.Common;
using Toolsfactory.Home.Adapters.Garbage.Awido;
using Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig;
using Toolsfactory.Home.Adapters.Heating.Wolf;
using Toolsfactory.Protocols.Homie.Devices;
using Tiveria.Common.Extensions;
using Toolsfactory.Home.Adapters.Powermeter.D0;
using Toolsfactory.Home.Adapters.Weather.WeatherLogger2;
using System.Collections;

namespace Toolsfactory.Home.Adapters.MultiHost
{
    class Program
    {
        /// <summary>
        /// WeatherStation Proxy translating api calls received from the WeatherStation into REST API calls to OpenHab Items
        /// </summary>
        /// <param name="debug">Flag to enable the debugging for the init phase (before logging is active)</param>
        /// <param name="configDirectory">set the configuration directory. Overwrites the default config directory value </param>
        /// <param name="configFile">set the name of the main config file. If this parameter is set and the file exists, it takes precedence over the configDirectory option and the default config directory</param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine("MultiHost for Home Automation");
            PrintAllEnvVars();

            var debugOption = new Option<bool>
            (new[] { "--debug", "-d" },
            description: "Enable extended debugging",
            getDefaultValue: () => false);

            var configDirOption = new Option<string>
            (new[] { "--configdir", "-c" },
             description: "Directory with the config files. In case it is not with the executeable");

            var servicesOption = new Option<string>(new[] { "--services", "-s" })
            {
                AllowMultipleArgumentsPerToken = true
            };

            var rootCmd = new RootCommand();
            AddSubCommand(rootCmd, "weather", debugOption, configDirOption);
            AddSubCommand(rootCmd, "garbage", debugOption, configDirOption);
            AddSubCommand(rootCmd, "powermeter", debugOption, configDirOption);
            AddSubCommand(rootCmd, "gasprices", debugOption, configDirOption);
            AddSubCommand(rootCmd, "heating", debugOption, configDirOption);
            AddSubCommand(rootCmd, "all", debugOption, configDirOption);
            rootCmd.Add(debugOption);
            rootCmd.Add(configDirOption);

            rootCmd.SetHandler(async (debugOptionValue, configDirOptionValue) =>
            {
                await InitializeAllAsync(debugOptionValue, configDirOptionValue, "all");
            }, debugOption, configDirOption);

            await rootCmd.InvokeAsync(args);
        }

        private static void PrintAllEnvVars()
        {
            Console.WriteLine("Env Variables");
            Console.WriteLine("-------------");
            Console.WriteLine();
            foreach(DictionaryEntry env in System.Environment.GetEnvironmentVariables())
            {
                Console.WriteLine($"{env.Key} = {env.Value}");
            }
            Console.WriteLine();
        }

        private static void AddSubCommand(RootCommand root, string subcmd, Option<bool> debugOpt, Option<string> configDirOpt)
        {
            var sub = new Command(subcmd);
            sub.SetHandler(async (debugOptionValue, configDirOptionValue) =>
            {
                await InitializeAllAsync(debugOptionValue, configDirOptionValue, subcmd);
            }, debugOpt, configDirOpt);
            root.Add(sub);
        }

        static async Task InitializeAllAsync(bool debug, string configDir, string command)
        {
            Console.WriteLine("MultiHost started for " +  command);
            var host = new ServiceHost<Program>("multihost", debug, configDir, "");
            await host.BuildBaslineHost(Array.Empty<string>())
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<HomieMqttServerConfiguration>(hostContext.Configuration.GetSection("MqttServer"));

                    if (command == "garbage" || command == "all")
                    { 
                        services
                            .Configure<AbfallkalenderOptions>(hostContext.Configuration.GetSection("services:garbage"))
                            .AddSingleton<IHostedService, AbfallKalenderService>();
                    }
                    if (command == "gasprices" || command == "all")
                    {
                        services
                            .Configure<GaspricesOptions>(hostContext.Configuration.GetSection("services:gasprices"))
                            .AddSingleton<IHostedService, GaspricesService>();
                    }
                    if (command == "heating" || command == "all")
                    {
                        services
                            .Configure<HeatingOptions>(hostContext.Configuration.GetSection("services:heating"))
                            .AddSingleton<IHostedService, HeatingService>();
                    }
                    if (command == "powermeter" || command == "all")
                    {
                        services
                            .Configure<PowermeterConfig>(hostContext.Configuration.GetSection("services:powermeter"))
                            .AddSingleton<IHostedService, PowermeterService>();
                    }
                    if (command == "weather" || command == "all")
                    {
                        services
                            .Configure<WeatherOptions>(hostContext.Configuration.GetSection("services:weather"))
                            .AddSingleton<IHostedService, WeatherService>();
                    }
                })
                .RunConsoleAsync().ConfigureAwait(false);
        }

    }
}
