using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.CommandLine;
using Toolsfactory.Home.Adapters.Common;
using Toolsfactory.Home.Adapters.Heating.Wolf.Service;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Heating.Wolf.Host
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
            var host = new ServiceHost<Program>("heating", debug, configDirectory, configFile);
            await host.BuildBaslineHost(Array.Empty<string>())
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<HomieMqttServerConfiguration>(hostContext.Configuration.GetSection("MqttServer"))
                        .Configure<HeatingOptions>(hostContext.Configuration.GetSection("services:heating"))
                        .AddSingleton<IHostedService, HeatingService>();
                })
                .RunConsoleAsync().ConfigureAwait(false);
        }
    }
}
