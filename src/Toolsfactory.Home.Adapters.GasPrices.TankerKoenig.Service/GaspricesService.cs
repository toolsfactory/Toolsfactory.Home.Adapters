using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig;
using Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig.DTO;
using Tiveria.Common;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Reflection;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig
{
    public class GaspricesService : BackgroundService
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<GaspricesService> _logger;
        private readonly IOptions<GaspricesOptions> _options;
        private readonly IOptions<HomieMqttServerConfiguration> _mqttConfig;
        private readonly BaseApi _api;
        private readonly Random _rand;
        private readonly int _delayvar;
        private readonly HomieEnvironmentBuilder _homieEnv;

        public GaspricesService(ILoggerFactory loggerfactory, IHttpClientFactory httpClientFactory, IOptions<GaspricesOptions> options, IOptions<HomieMqttServerConfiguration> mqttConfig)
        {
            Ensure.That(loggerfactory).IsNotNull();
            Ensure.That(options).IsNotNull();
            Ensure.That(options.Value).IsNotNull().WithExtraMessageOf(() => "Options Value");
            Ensure.That(options.Value.Tankerkoenig).IsNotNull().WithExtraMessageOf(() => "Tankerkoenig");
            Ensure.That(httpClientFactory).IsNotNull();

            _loggerFactory = loggerfactory;
            _logger = loggerfactory.CreateLogger<GaspricesService>();
            _options = options;
            _mqttConfig = mqttConfig;
            _logger.LogInformation("Creating service");
            _api = new BaseApi(options.Value.Tankerkoenig.ApiKey, new Uri(options.Value.Tankerkoenig.BaseUrl), httpClientFactory, _logger);
            _rand = new Random(564);
            _delayvar = _options.Value.UpdateDelaySeconds / 10;
            _homieEnv = new HomieEnvironmentBuilder(_options.Value.HomieDeviceIdentifier, _options.Value.HomieDeviceName, mqttConfig.Value, options.Value.Tankerkoenig.GasStations, _loggerFactory);

            _logger.LogDebug("Service created");
            _logger.LogDebug("Baseline configuration : {@Configuration}", _options.Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("GaspricesService starting");

            stoppingToken.Register(() =>
                _logger.LogInformation("Service stop request received."));

            await Task.Delay(millisecondsDelay: _options.Value.StartupDelaySeconds * 1000, cancellationToken: stoppingToken).ConfigureAwait(false);

            var ids = _options.Value.Tankerkoenig.GasStations.Select(x => x.StationId).ToArray();
            await _homieEnv.StartAsync();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Updating prices");
                try
                {
                    var data = await _api.GetPriceListAsync(ids).ConfigureAwait(false);
                    if(data.Ok)
                        await PublishInfoAsync(data, stoppingToken).ConfigureAwait(false);
                    else
                    {
                        _logger.LogError($"Received error from Tankerkoenig: {data.Message}");
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e, "Execution of price update failed");
                }
                var nextdelay = GenerateNextDelay();
                _logger.LogInformation($"Next update in : {nextdelay/1000} sec");
                await Task.Delay(millisecondsDelay: nextdelay, cancellationToken: stoppingToken).ConfigureAwait(false);
            }

            await _homieEnv.StopAsync();
            await Task.Delay(1000, CancellationToken.None).ConfigureAwait(false);
            _logger.LogInformation("Service is finished.");

            int GenerateNextDelay()
            {
                return (_options.Value.UpdateDelaySeconds + _rand.Next(-_delayvar, _delayvar)) * 1000;
            }
        }

        private async Task PublishInfoAsync(PriceListDto data, CancellationToken token)
        {
            await Task.Run(() =>
            {
                foreach (var gasprices in data.Prices)
                {
                    var gsSettings = _options.Value.Tankerkoenig.GasStations.FirstOrDefault(x => x.StationId.ToUpperInvariant() == gasprices.Key.ToUpperInvariant());
                    if (gsSettings != null)
                    {
                        var id = gsSettings.StationId.ToLowerInvariant();
                        if (_homieEnv.MappedProperties.TryGetValue(id + "-diesel", out var propD))
                        {
                            propD.Value = (double) gasprices.Value.Diesel.Value;
                            _logger.DebugGasPriceChange(gasprices.Value.Diesel.Value, "Diesel", id);
                        }
                        if (_homieEnv.MappedProperties.TryGetValue(id + "-supere5", out var propE5))
                        {
                            propE5.Value = (double) gasprices.Value.E5;
                            _logger.DebugGasPriceChange(gasprices.Value.E5.Value, "Super E5", id);
                        }
                        if (_homieEnv.MappedProperties.TryGetValue(id + "-supere10", out var propE10))
                        {
                            propE10.Value = (double) gasprices.Value.E10;
                            _logger.DebugGasPriceChange(gasprices.Value.E10.Value, "Super E10", id);
                        }
                    }
                }
            });
        }
    }

    #region logging helper class for high performance logging (using static methods as in Microsoft core libs)
    internal static class Log
    {
        private static Action<ILogger, float, string, string, Exception> _logGaspriceChange = LoggerMessage.Define<float, string, string>(
            LogLevel.Debug,
            new EventId(1),
            "Set '{value}' for {gas} on {stationid}");
        public static ILogger DebugGasPriceChange(this ILogger logger, float value, string gas, string station)
        {
            _logGaspriceChange(logger, value, gas, station, null);
            return logger;
        }
    }

    #endregion
}
