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
            _logger.LogInformation("Service is starting.");

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
                foreach (var Gasprices in data.Prices)
                {
                    var gsSettings = _options.Value.Tankerkoenig.GasStations.FirstOrDefault(x => x.StationId.ToUpperInvariant() == Gasprices.Key.ToUpperInvariant());
                    if (gsSettings != null)
                    {
                        var id = gsSettings.StationId.ToLowerInvariant();
                        if (_homieEnv.MappedProperties.TryGetValue(id + "-diesel", out var propD))
                        {
                            propD.Value = (double) Gasprices.Value.Diesel;
                        }
                        if (_homieEnv.MappedProperties.TryGetValue(id + "-supere5", out var propE5))
                        {
                            propE5.Value = (double) Gasprices.Value.E5;
                        }
                        if (_homieEnv.MappedProperties.TryGetValue(id + "-supere10", out var propE10))
                        {
                            propE10.Value = (double) Gasprices.Value.E10;
                        }
                    }
                }
            });
        }


        #region Logging helpers

        private static Action<ILogger, string, Uri, Exception> _updateSentToEndpoint = LoggerMessage.Define<string, Uri>(
            LogLevel.Information,
            new EventId(1),
            "Sent '{value}' to '{endpoint}'");

        #endregion

    }

    public class ObjectDumper
    {
        private int _level;
        private readonly int _indentSize;
        private readonly StringBuilder _stringBuilder;
        private readonly List<int> _hashListOfFoundElements;

        private ObjectDumper(int indentSize)
        {
            _indentSize = indentSize;
            _stringBuilder = new StringBuilder();
            _hashListOfFoundElements = new List<int>();
        }

        public static string Dump(object element)
        {
            return Dump(element, 2);
        }

        public static string Dump(object element, int indentSize)
        {
            var instance = new ObjectDumper(indentSize);
            return instance.DumpElement(element);
        }

        private string DumpElement(object element)
        {
            if (element == null || element is ValueType || element is string)
            {
                Write(FormatValue(element));
            }
            else
            {
                var objectType = element.GetType();
                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    Write("{{{0}}}", objectType.FullName);
                    _hashListOfFoundElements.Add(element.GetHashCode());
                    _level++;
                }

                var enumerableElement = element as IEnumerable;
                if (enumerableElement != null)
                {
                    foreach (object item in enumerableElement)
                    {
                        if (item is IEnumerable && !(item is string))
                        {
                            _level++;
                            DumpElement(item);
                            _level--;
                        }
                        else
                        {
                            if (!AlreadyTouched(item))
                                DumpElement(item);
                            else
                                Write("{{{0}}} <-- bidirectional reference found", item.GetType().FullName);
                        }
                    }
                }
                else
                {
                    MemberInfo[] members = element.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var memberInfo in members)
                    {
                        var fieldInfo = memberInfo as FieldInfo;
                        var propertyInfo = memberInfo as PropertyInfo;

                        if (fieldInfo == null && propertyInfo == null)
                            continue;

                        var type = fieldInfo != null ? fieldInfo.FieldType : propertyInfo.PropertyType;
                        object value = fieldInfo != null
                                           ? fieldInfo.GetValue(element)
                                           : propertyInfo.GetValue(element, null);

                        if (type.IsValueType || type == typeof(string))
                        {
                            Write("{0}: {1}", memberInfo.Name, FormatValue(value));
                        }
                        else
                        {
                            var isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
                            Write("{0}: {1}", memberInfo.Name, isEnumerable ? "..." : "{ }");

                            var alreadyTouched = !isEnumerable && AlreadyTouched(value);
                            _level++;
                            if (!alreadyTouched)
                                DumpElement(value);
                            else
                                Write("{{{0}}} <-- bidirectional reference found", value.GetType().FullName);
                            _level--;
                        }
                    }
                }

                if (!typeof(IEnumerable).IsAssignableFrom(objectType))
                {
                    _level--;
                }
            }

            return _stringBuilder.ToString();
        }

        private bool AlreadyTouched(object value)
        {
            if (value == null)
                return false;

            var hash = value.GetHashCode();
            for (var i = 0; i < _hashListOfFoundElements.Count; i++)
            {
                if (_hashListOfFoundElements[i] == hash)
                    return true;
            }
            return false;
        }

        private void Write(string value, params object[] args)
        {
            var space = new string(' ', _level * _indentSize);

            if (args != null)
                value = string.Format(value, args);

            _stringBuilder.AppendLine(space + value);
        }

        private string FormatValue(object o)
        {
            if (o == null)
                return ("null");

            if (o is DateTime)
                return (((DateTime)o).ToShortDateString());

            if (o is string)
                return string.Format("\"{0}\"", o);

            if (o is char && (char)o == '\0')
                return string.Empty;

            if (o is ValueType)
                return (o.ToString());

            if (o is IEnumerable)
                return ("...");

            return ("{ }");
        }
    }
}
