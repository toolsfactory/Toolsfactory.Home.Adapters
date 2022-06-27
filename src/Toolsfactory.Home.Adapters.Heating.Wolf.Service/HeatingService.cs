using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tiveria.Common;
using Tiveria.Common.Extensions;
using Tiveria.Home.Knx.Datapoint;
using Tiveria.Home.Knx.IP;
using Tiveria.Home.Knx.IP.Structures;
using Tiveria.Home.Knx.ObjectServer;

namespace Toolsfactory.Home.Adapters.Heating.Wolf.Service
{
    public class HeatingService : BackgroundService
    {
        private static byte[] GETALL = new byte[] { 0x06, 0x20, 0xF0, 0x80, 0x00, 0x16, 0x04, 0x00, 0x00, 0x00, 0xF0, 0xD0 };


        #region private fields
        private readonly ILogger<HeatingService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IOptions<HeatingOptions> _options;
        #endregion

        #region constructor
        public HeatingService(ILoggerFactory loggerfactory, IHttpClientFactory httpClientFactory, IOptions<HeatingOptions> options)
        {
            Ensure.That(loggerfactory).IsNotNull();
            Ensure.That(options).IsNotNull();
            Ensure.That(options.Value).IsNotNull();

            _options = options;
            _logger = loggerfactory.CreateLogger<HeatingService>();
            _httpClient = httpClientFactory.CreateClient("HeatingService");
        }
        #endregion
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service ready to start with delay: {startupdelay} sec", _options.Value.StartupDelaySeconds);

            await Task.Delay(millisecondsDelay: _options.Value.StartupDelaySeconds * 1000, cancellationToken: cancellationToken).ConfigureAwait(false);

            await base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await base.StopAsync(cancellationToken);
            await Task.Delay(1000).ConfigureAwait(false);
            _logger.LogInformation("Service finished.");
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Service started.");
            cancellationToken.Register(() => _logger.LogInformation("Service stop request received."));
            TcpListener server = new TcpListener(new IPEndPoint(IPAddress.Any, _options.Value.LocalServer.Port));

            server.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                _logger.LogDebug("Waiting for connection requests");
                TcpClient client = await server.AcceptTcpClientAsync();
                _logger.LogDebug("Remote Endpoint: {RemoteEndPoint}", client.Client.RemoteEndPoint);
                NetworkStream stream = client.GetStream();
                int inputlen = 0;
                stream.Write(GETALL);
                var input = new byte[1024];
                var buffer = new byte[0];
                // Loop to receive all the data sent by the client.
                while ((inputlen = await stream.ReadAsync(input, 0, input.Length, cancellationToken)) != 0)
                {
                    buffer = buffer.Merge(input, inputlen);

                    while (TryParseData(out var req, out var size, buffer))
                    {
                        var header = new ConnectionHeader();
                        var frame = new KnxNetIPFrame(new ObjectServerProtocolService(header, new SetDatapointValueResService(req.StartDataPoint, 0)));
                        var answer = frame.ToBytes();
                        stream.Write(answer);
                        foreach (var item in req.DataPoints)
                        {
                            if (Datapoints.TryGetValue(item.ID, out var dp))
                            {
                                var dpttype = DatapointTypesList.GetTypeById(dp.dptid);
                                if (dpttype != null)
                                {
                                    var value = dpttype.DecodeString(item.Value, withUnit: false, invariant: true);
                                    var key = dp.device + "_" + dp.dptname;
                                    await PutUpdateAsync(key, value);
                                    //                                    _logger.LogDebug("Sent {value} to {key}", value, key);
                                }
                            }
                        }
                        buffer = buffer.Clone(size);
                        //await PutUpdateAsync(_options.Value.OHServer.ItemLastUpdate, DateTime.Now.ToString("s", CultureInfo.InvariantCulture.DateTimeFormat));
                    }
                }

                // Shutdown and end connection
                client.Close();
                _logger.LogDebug("Connection closed");
            }
            server.Stop();
            _logger.LogDebug("Server stopped");
        }

        async Task PutUpdateAsync(string key, string value)
        {
            try
            {
                var ep = GenerateUriForItem(key);
                var result = await _httpClient.PutAsync(ep, new StringContent(value));
                _logger.UpdateSent(ep, value);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error sending update");
            }
        }

        #region internal helper to generate item uri
        private Uri GenerateUriForItem(string item)
        {
            //var path = String.Format(CultureInfo.InvariantCulture, _options.Value.OHServer.Basepath, item);
            return new Uri(item);
        }
        #endregion

        private static bool TryParseData(out SetDatapointValueReqService? result, out int size, Span<byte> data)
        {
            size = 0;
            if (KnxNetIPFrame.TryParse(data.ToArray(), out var frame))
            {
                size = frame.FrameHeader.TotalLength;
                var os = frame.Service as ObjectServerProtocolService;
                if (os != null)
                {
                    result = os.ObjectServerService as SetDatapointValueReqService;
                    return result != null;
                }
            }
            result = null;
            return false;
        }
    }

    #region logging helper class for high performance logging (using static methods as in Microsoft core libs)
    internal static class Log
    {
        private static Action<ILogger, string, Uri, Exception> _updateSentToEndpoint = LoggerMessage.Define<string, Uri>(
            LogLevel.Information,
            new EventId(1),
            "Sent '{value}' to '{endpoint}'");
        public static ILogger UpdateSent(this ILogger logger, Uri endpoint, string value)
        {
            _updateSentToEndpoint(logger, value, endpoint, null);
            return logger;
        }
    }

#endregion
}
