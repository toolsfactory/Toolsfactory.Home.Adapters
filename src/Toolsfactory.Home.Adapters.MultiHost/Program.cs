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
            Console.WriteLine("Hello, World!");

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

            var singleGarbageCmd = new Command("garbage");
            var singleWeatherCmd = new Command("weather");
            var singlePowermeterCmd = new Command("powermeter");
            var singleGaspricesCmd  = new Command("gasprices");
            var allCmd = new Command("all", "starts all known services");

            var rootCmd = new RootCommand()
            {
                allCmd,
                singleGarbageCmd,
                singleWeatherCmd,
                singlePowermeterCmd,
                singleGaspricesCmd,
                debugOption,
                configDirOption
            };
            allCmd.SetHandler(async (debugOptionValue, configDirOptionValue) =>
            {
                await InitializeAllAsync(debugOptionValue, configDirOptionValue, "");
            }, debugOption, configDirOption);

            singleGarbageCmd.SetHandler(async (debugOptionValue, configDirOptionValue) =>
            {
                await InitializeAllAsync(debugOptionValue, configDirOptionValue, "garbage");
            }, debugOption, configDirOption);

            singleWeatherCmd.SetHandler(async (debugOptionValue, configDirOptionValue) =>
            {
                await InitializeAllAsync(debugOptionValue, configDirOptionValue, "weather");
            }, debugOption, configDirOption);

            singleGaspricesCmd.SetHandler(async (debugOptionValue, configDirOptionValue) =>
            {
                await InitializeAllAsync(debugOptionValue, configDirOptionValue, "gasprices");
            }, debugOption, configDirOption);

            singlePowermeterCmd.SetHandler(async (debugOptionValue, configDirOptionValue) =>
            {
                await InitializeAllAsync(debugOptionValue, configDirOptionValue, "powermeter");
            }, debugOption, configDirOption);

            await rootCmd.InvokeAsync(args);
        }

        static async Task InitializeAllAsync(bool debug, string configDir, string command)
        {
            Console.WriteLine("MultiHost started for " + (command.IsNullOrEmpty() ? "all services" : command));
            var host = new ServiceHost<Program>("multihost", debug, configDir, "");
            await host.BuildBaslineHost(Array.Empty<string>())
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<HomieMqttServerConfiguration>(hostContext.Configuration.GetSection("MqttServer"));

                    if (command == "garbage" || command.IsNullOrEmpty())
                    { 
                        services
                            .Configure<AbfallkalenderOptions>(hostContext.Configuration.GetSection("services:garbage"))
                            .AddSingleton<IHostedService, AbfallKalenderService>();
                    }
                    if (command == "gasprices" || command.IsNullOrEmpty())
                    {
                        services
                            .Configure<GaspricesOptions>(hostContext.Configuration.GetSection("services:gasprices"))
                            .AddSingleton<IHostedService, GaspricesService>();
                    }
                    if (command == "heating" || command.IsNullOrEmpty())
                    {
                        services
                            .Configure<HeatingOptions>(hostContext.Configuration.GetSection("services:heating"))
                            .AddSingleton<IHostedService, HeatingService>();
                    }
                    if (command == "powermeter" || command.IsNullOrEmpty())
                    {
                        services
                            .Configure<PowermeterConfig>(hostContext.Configuration.GetSection("services:powermeter"))
                            .AddSingleton<IHostedService, PowermeterService>();
                    }
                    if (command == "weather" || command.IsNullOrEmpty())
                    {
                        services
                            .Configure<WeatherOptions>(hostContext.Configuration.GetSection("services:weather"))
                            .AddSingleton<IHostedService, WeatherProxyService>();
                    }
                })
                .RunConsoleAsync().ConfigureAwait(false);
        }

    }
}
