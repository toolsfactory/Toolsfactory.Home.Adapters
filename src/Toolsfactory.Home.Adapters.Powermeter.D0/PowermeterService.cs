using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using Tiveria.Common;
using Toolsfactory.Protocols.D0;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Powermeter.D0
{
    public class PowermeterService : BackgroundService
    {
        private readonly ILogger _logger;
        private readonly Tiveria.Common.Logging.HexDumpLogger _hexLogger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IOptions<PowermeterConfig> _options;
        private readonly HomieEnvironmentBuilder _homieEnv;

        private D0SerialTransport? _transport;
        private double _lastValue = -1;
        private bool _started = false;

        public PowermeterService(ILoggerFactory loggerFactory, IOptions<PowermeterConfig> options, IOptions<HomieMqttServerConfiguration> mqttConfig, IHostApplicationLifetime hostApplicationLifetime)
        {
            Ensure.That(loggerFactory, nameof(loggerFactory)).IsNotNull();
            Ensure.That(options, nameof(options)).IsNotNull();
            _loggerFactory = loggerFactory;
            _options = options;
            _hostApplicationLifetime = hostApplicationLifetime;
            _logger = loggerFactory.CreateLogger<PowermeterService>();
            _hexLogger = new Tiveria.Common.Logging.HexDumpLogger(new LoggingExtensionsLogger(_logger));
            _homieEnv = new HomieEnvironmentBuilder(options.Value.HomieDeviceIdentifier, options.Value.HomieDeviceName, mqttConfig.Value, _loggerFactory);
        }

        public override void Dispose()
        {
            _logger.LogInformation("Disposing DaemonService...");
            base.Dispose();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"PowermeterService is starting.");
            await Task.Delay(1000, cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Startup delay: {startupdelay} secs", _options.Value.DelaySec);

            if (!TestTransportAndPort(_options.Value.SerialPort))
            {
                _logger.LogCritical("Error starting serial device. Service not started.");
                return;
            }
            await _homieEnv.StartAsync();
            await base.StartAsync(cancellationToken);
            _started = true;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_started)
                return;

            stoppingToken.Register(() =>
                _logger.LogDebug("DeamonService stop request received."));
    
            await RunLoopAsync(stoppingToken).ConfigureAwait(false);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);

            await Task.Delay(1000).ConfigureAwait(false);
            _logger.LogDebug($"DaemonService is finished.");

        }

        private async Task RunLoopAsync(CancellationToken stoppingToken)
        {
            Int64 counter = 0;

            Ensure.That(_transport).IsNotNull();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"Beginning with loop {counter++}");
                    _transport!.Open();
                    var parser = CreateParser(stoppingToken);
                    var ok = await parser.ReadAndParseAsync().ConfigureAwait(false);
                    _transport!.Close();
                    Thread.Sleep(5 * 60 * 1000);
                }
                catch (Exception e)
                {
                    _hexLogger.Flush();
                    _logger.LogError("Exception in read loop", e);
                    Thread.Sleep(5000);
                }
                finally
                {
                    if (_transport!.IsOpen)
                        _transport!.Close();
                }
            }
        }

        private D0SimpleStreamParser CreateParser(CancellationToken stoppingToken)
        {
            var parser = new D0SimpleStreamParser(_loggerFactory.CreateLogger<D0SimpleStreamParser>(), _hexLogger, _transport, stoppingToken, false);
            parser.VendorMessageEvent += Parser_VendorMessageEvent;
            parser.ObisDataEvent += Parser_ObisDataEvent;
            return parser;
        }

        private bool TestTransportAndPort(string serialDev)
        {
            _transport = new D0SerialTransport(_loggerFactory.CreateLogger<D0SerialTransport>(), serialDev);
           try
            {
                _transport.Open();
                _transport.Close();
                _logger.LogInformation("Serial device sucessfully tested");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Serial device could not be opened!", e);
                return false;
            }
        }

        private void Parser_ObisDataEvent(object sender, ObisDataEventArgs e)
        {
            _logger.LogInformation($"Obis Data - Code: {e.Code} - Value: {e.Value} - Unit: {e.Unit}");
            if (e.Code == "1.8.0")
                _ = ReactOn180ObisDataAsync(e.Value);
        }

        private void Parser_VendorMessageEvent(object sender, VendorMessageEventArgs e)
        {
            _logger.LogInformation($"Vendor Info - Name: {e.Vendor} - Indentification: {e.Identification} - BaudChar: {e.BaudrateCharacter} ({e.Baudrate})");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2234:System-URI-Objekte anstelle von Zeichenfolgen übergeben", Justification = "<Ausstehend>")]
        private async Task ReactOn180ObisDataAsync(string value)
        {
            if (double.TryParse(value, System.Globalization.NumberStyles.Float, CultureInfo.InvariantCulture, out var val))
            {
                _homieEnv.TotalConsumptionProperty.Value = val;
                if (_lastValue > -1 && _lastValue <= val)
                {
                    var period = (val - _lastValue) * 1000;
                    _logger.LogInformation($"Calculating Period: ({val} - {_lastValue}) * 1000 = {period}");
                    _homieEnv.PeriodConsumptionProperty.Value = period;
                }
                _lastValue = val;
            }
        }
    }
}
