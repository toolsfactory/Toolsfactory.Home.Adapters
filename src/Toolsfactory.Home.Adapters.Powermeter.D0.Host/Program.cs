using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Toolsfactory.Home.Adapters.Common;
using Toolsfactory.Protocols.Homie.Devices;
using Toolsfactory.Home.Adapters.Powermeter.D0;

namespace Tiveria.Home.D0.SampleApp
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
        static async Task Main(bool debug = false, string? configDirectory = null, string? configFile = null)
        {
            var host = new ServiceHost<Program>("d0powermeter", debug, configDirectory, configFile);
            await host.BuildBaslineHost(Array.Empty<string>())
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<HomieMqttServerConfiguration>(hostContext.Configuration.GetSection("MqttServer"))
                        .Configure<PowermeterConfig>(hostContext.Configuration.GetSection("services:d0powermeter"))
                        .AddSingleton<IHostedService, PowermeterService>();
                })
                .RunConsoleAsync().ConfigureAwait(false);
        }
    }
}
