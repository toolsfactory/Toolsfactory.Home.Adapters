using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Toolsfactory.Home.Adapters.Common;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new ServiceHost<Program>("Gasprices");
            await host.BuildBaslineHost(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<HomieMqttServerConfiguration>(hostContext.Configuration.GetSection("MqttServer"))
                        .Configure<GaspricesOptions>(hostContext.Configuration.GetSection("services:gasprices"))
                        .AddSingleton<IHostedService, GaspricesService>();
                })
                .RunConsoleAsync().ConfigureAwait(false);
        }
    }
}
