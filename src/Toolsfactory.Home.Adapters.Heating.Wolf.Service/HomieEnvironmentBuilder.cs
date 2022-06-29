using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tiveria.Common.Extensions;
using Toolsfactory.Protocols.Homie.Devices;
using Toolsfactory.Protocols.Homie.Devices.Properties;

namespace Toolsfactory.Home.Adapters.Heating.Wolf.Service
{
    public record PropertyEntry(int Id, string DptId, BaseProperty Property);
    public class HomieEnvironmentBuilder
    {
        private const string FormatPropertyKey = "nextformat";

        private readonly string _deviceId;
        private readonly string _deviceName;
        private readonly List<Node>? _homieDeviceNodes;
        private readonly IReadOnlyDictionary<string, string> _categoriesMapping;
        private readonly HomieMqttServerConfiguration _homieHostConfig;
        private readonly ILoggerFactory _loggerFactory;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public HomieDeviceHost DeviceHost { get; private set; }
        public Device RootDevice { get; private set; }
        public Dictionary<int, PropertyEntry> MappedProperties { get; private set; } = new Dictionary<int, PropertyEntry>();
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
                        if (DPT2HomieDataConverter.TryGetHomiePropertyTypeFromDptId(property.DptId, out var htype))
                        {
                            BaseProperty? homieprop = null;
                            switch (htype)
                            {
                                case "boolean": homieprop = new BooleanProperty(homienode, property.DptName.ToLowerInvariant(), property.Name, settable: property.Writeable); break;
                                case "integer": homieprop = new IntegerProperty(homienode, property.DptName.ToLowerInvariant(), property.Name, settable: property.Writeable); break;
                                case "float": homieprop = new FloatProperty(homienode, property.DptName.ToLowerInvariant(), property.Name, settable: property.Writeable); break;
                                    default: break;
                            }

                            if (homieprop != null)
                            {
                                homienode.AddProperty(homieprop);
                                MappedProperties.Add(property.Id, new(property.Id, property.DptId, homieprop));
                            }
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

