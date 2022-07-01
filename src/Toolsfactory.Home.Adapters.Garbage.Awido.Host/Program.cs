using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Toolsfactory.Home.Adapters.Common;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Garbage.Awido.Host
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new ServiceHost<Program>("garbage");
            await host.BuildBaslineHost(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services
                        .Configure<HomieMqttServerConfiguration>(hostContext.Configuration.GetSection("MqttServer"))
                        .Configure<AbfallkalenderOptions>(hostContext.Configuration.GetSection("services:garbage"))
                        .AddSingleton<IHostedService, AbfallKalenderService>();
                })
                .RunConsoleAsync().ConfigureAwait(false);
        }
    }
}
