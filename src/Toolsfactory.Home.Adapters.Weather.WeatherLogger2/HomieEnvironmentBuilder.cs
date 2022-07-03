using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tiveria.Common.Extensions;
using Toolsfactory.Protocols.Homie.Devices;
using Toolsfactory.Protocols.Homie.Devices.Properties;

namespace Toolsfactory.Home.Adapters.Weather.WeatherLogger2
{
    public record PropertyEntry(int Id, string DptId, BaseProperty Property);
    public class HomieEnvironmentBuilder
    {
        private readonly string _deviceId;
        private readonly string _deviceName;
        private readonly List<Node>? _homieDeviceNodes;
        private readonly HomieMqttServerConfiguration _homieHostConfig;
        private readonly ILoggerFactory _loggerFactory;

        public HomieDeviceHost DeviceHost { get; private set; }
        public Device RootDevice { get; private set; }
        public Dictionary<string, BaseProperty> MappedProperties { get; private set; } = new ();
        public bool IsStarted { get { return (DeviceHost == null) ? false : DeviceHost.IsStarted; } }


        public HomieEnvironmentBuilder(string deviceId, string deviceName, List<Node>? homieDeviceNodes, HomieMqttServerConfiguration homieHostConfig, ILoggerFactory loggerFactory)
        {
            _deviceId = deviceId;
            _deviceName = deviceName;
            _homieDeviceNodes = homieDeviceNodes;
            _homieHostConfig = homieHostConfig;
            _loggerFactory = loggerFactory;
            CreateDevice();
            CreateHost();
        }

        public async Task StartAsync()
        {
            await DeviceHost.StartAsync();
        }

        public async Task StopAsync()
        {
            await DeviceHost.StopAsync();
        }

        private void CreateDevice()
        {
            RootDevice = new Device(_deviceId, _deviceName);
            AddNodesProperties();
        }

        private void AddNodesProperties()
        {
            if (_homieDeviceNodes == null)
                return;

            foreach(var node in _homieDeviceNodes)
            {
                var homienode = RootDevice.AddNode(node.Name);
                if (node.Properties != null)
                {
                    foreach (var property in node.Properties)
                    {
                        BaseProperty? homieprop = null;
                        switch (property.Type)
                        {
                            case "boolean": homieprop = new BooleanProperty(homienode, property.Name.ToLowerInvariant(), property.Name); break;
                            case "integer": homieprop = new IntegerProperty(homienode, property.Name.ToLowerInvariant(), property.Name); break;
                            case "float": homieprop = new FloatProperty(homienode, property.Name.ToLowerInvariant(), property.Name); break;
                            case "datetime": homieprop = new  DateTimeProperty(homienode, property.Name.ToLowerInvariant(), property.Name); break;
                            default: break;
                        }

                        if (homieprop != null)
                        {
                            homienode.AddProperty(homieprop);
                            MappedProperties.Add(property.SourceName, homieprop);
                        }
                    }
                }
            }
        }

        private void CreateHost()
        {
            DeviceHost = new HomieDeviceHost(RootDevice, _homieHostConfig, _loggerFactory.CreateLogger<HomieDeviceHost>());
        }
    }
}

