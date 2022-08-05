using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Net;
using Tiveria.Common;
using Tiveria.Common.BasicHttpServer;
using Tiveria.Common.Extensions;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Weather.WeatherLogger2
{
    public class WeatherService : BackgroundService
    {
        #region private fields
        private readonly ILogger<WeatherService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IOptions<WeatherOptions> _options;
        private readonly Tiveria.Common.BasicHttpServer.HttpServer _server;
        private readonly HomieEnvironmentBuilder _homieEnv;
        #endregion

        #region constructor
        public WeatherService(ILoggerFactory loggerfactory, IHttpClientFactory httpClientFactory, IOptions<WeatherOptions> options, IOptions<HomieMqttServerConfiguration> mqttConfig)
        {
            Ensure.That(loggerfactory).IsNotNull();
            Ensure.That(httpClientFactory).IsNotNull();
            Ensure.That(options).IsNotNull();
            Ensure.That(options.Value).IsNotNull();

            _options = options;
            _logger = loggerfactory.CreateLogger<WeatherService>();
            _httpClient = httpClientFactory.CreateClient("Weatherserviceproxy");
            _server = new HttpServer(loggerfactory.CreateLogger<HttpServer>(), _options.Value.LocalServer.Port);
            _homieEnv = new HomieEnvironmentBuilder(_options.Value.HomieDeviceIdentifier, _options.Value.HomieDeviceName, _options.Value.HomieDeviceNodes, mqttConfig.Value, loggerfactory);
        }
        #endregion

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("WeatherService starting");
            _logger.LogInformation("Service ready to start with delay: {startupdelay} sec", _options.Value.StartupDelaySeconds);

            await Task.Delay(millisecondsDelay: _options.Value.StartupDelaySeconds * 1000, cancellationToken: cancellationToken).ConfigureAwait(false);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started.");
            cancellationToken.Register(() => _logger.LogInformation("Service stop request received."));
            await _homieEnv.StartAsync();
            await _server.StartAsync(OnHttpRequestAsync, cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _homieEnv.StopAsync();
            await base.StopAsync(cancellationToken);
            await Task.Delay(1000).ConfigureAwait(false);
            _logger.LogInformation("Service finished.");
        }

        Task OnHttpRequestAsync(HttpListenerRequest request, HttpListenerResponse response)
        {
            return Task.Run(() =>
            {
                _logger.LogInformation("New request: {request}", request.Url);
                response.StatusCode = (int) HttpStatusCode.OK;
                response.AsText("Success");
                response.Close();
                _logger.LogDebug("Request handled");
                SendUpdatesAsync(request).Wait();
            });
        }

        private async Task SendUpdatesAsync(HttpListenerRequest request)
        {
            foreach (var item in request.QueryString.AllKeys)
            {
                if (item == "ID" || item == "PASSWORD" || item == "action") continue;
                var value = TranslateValue(item, request.QueryString[item]);
                await PutUpdateAsync(item, value);
                if (item == "winddir" && double.TryParse(value, out var deg))
                {
                    await PutUpdateAsync("winddirname", degrees2name(deg));
                }
            }
        }

        async Task PutUpdateAsync(string key, string value)
        {
            if (_homieEnv.MappedProperties.TryGetValue(key, out var properties))
            {
                properties.RawValue = value;
                _logger.DebugParameterSent(key, value);
            }
        }

        #region internal helpers to convert values
        string TranslateValue(string item, string data)
        {
            var parsed = double.TryParse(data, NumberStyles.Float, CultureInfo.InvariantCulture, out var pdata);
            if (!parsed)
                return data;

            double value = pdata;
            if (item == "windchillf" || item == "dewptf" || item == "tempf" || item == "indoortempf")
            {
                value = pdata.Fahrenheit2Celsius();
            }
            else if (item == "baromin")
            {
                value = pdata.InHg2mBar();
            }
            else if (item.IndexOf("rainin") > -1)
            {
                value = pdata.Inch2MM();
            }
            else if (item.IndexOf("mph") > -1)
            {
                value = pdata.Mile2KM();
            }
            return value.ToString(CultureInfo.InvariantCulture);
        }

        private static readonly string[] degNames = new string[16] { "N", "NNO", "NO", "ONO", "O", "OSO", "SO", "SSO", "S", "SSW", "SW", "WSW", "W", "WNW", "NW", "NNW" };
        static string degrees2name(double deg)
        {
            var val = (int)Math.Floor((deg / 22.5) + 0.5);
            return degNames[(val % 16)];
        }
        #endregion
    }

    #region logging helper class for high performance logging (using static methods as in Microsoft core libs)
    internal static class Log
    {
        private static Action<ILogger, string, object, Exception> debugParameterSent = LoggerMessage.Define<string, object>(
            LogLevel.Debug,
            new EventId(1),
            "Set '{key}' to '{value}'");
        public static ILogger DebugParameterSent(this ILogger logger, string key, object value)
        {
            debugParameterSent(logger, key, value, null);
            return logger;
        }
    }
    #endregion
}
