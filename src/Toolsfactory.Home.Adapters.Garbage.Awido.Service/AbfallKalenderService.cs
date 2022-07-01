using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Garbage.Awido
{
    public class AbfallKalenderService : BackgroundService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<AbfallKalenderService> _logger;
        private readonly IOptions<AbfallkalenderOptions> _serviceOptions;
        private readonly IOptions<HomieMqttServerConfiguration> _mqttConfig;
        private readonly System.Timers.Timer _timer = new System.Timers.Timer();
        private HomieEnvironmentBuilder _homieEnv;
        private DateTime _lastFilesDownload;

        public AbfallKalenderService(ILoggerFactory loggerFactory, IOptions<AbfallkalenderOptions> serviceOptions, IOptions<HomieMqttServerConfiguration> mqttConfig)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<AbfallKalenderService>();
            _serviceOptions = serviceOptions;
            _mqttConfig = mqttConfig;
            _lastFilesDownload = DateTime.Now.AddMonths(-2);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("Service is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug("Service stop request received."));

            _homieEnv = new HomieEnvironmentBuilder(_serviceOptions.Value.HomieDeviceIdentifier, _serviceOptions.Value.HomieDeviceName, _serviceOptions.Value.CalendarCategoriesMapping, _mqttConfig.Value, _serviceOptions.Value.CalendarLoader, _loggerFactory);

            _logger.LogInformation($"Startup delay configured: {_serviceOptions.Value.StartupDelaySeconds} sec");
            await Task.Delay(millisecondsDelay: _serviceOptions.Value.StartupDelaySeconds * 1000, cancellationToken: stoppingToken).ConfigureAwait(false);

            await _homieEnv.StartAsync();
            _lastFilesDownload = DateTime.Now;
            StartTimer();

            stoppingToken.WaitHandle.WaitOne();

            _timer.Stop();
            await Task.Delay(1000).ConfigureAwait(false);
            _logger.LogDebug("Service is finished.");


            await _homieEnv.StopAsync();
        }

        private void StartTimer()
        {
            _timer.Interval = _serviceOptions.Value.TimerIntervalSeconds * 1000;
            _timer.Elapsed += _timer_Elapsed;
            _timer.Start();
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _homieEnv.UpdateItems();
            if (DateTime.Now.Month != _lastFilesDownload.Month)
            {
                _homieEnv.ReloadCalendarsAsync().Wait();
                _lastFilesDownload = DateTime.Now;
            }
            _logger.LogInformation("Timer triggered");
        }
    }
}