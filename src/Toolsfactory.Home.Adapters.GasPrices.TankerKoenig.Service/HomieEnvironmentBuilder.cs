using System;
using System.Collections.Generic;
using Toolsfactory.Protocols.Homie.Devices;
using Toolsfactory.Protocols.Homie.Devices.Properties;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig
{
    public class HomieEnvironmentBuilder
    {
        private const string FormatPropertyKey = "nextformat";

        private readonly string _deviceId;
        private readonly string _deviceName;
        private readonly HomieMqttServerConfiguration _homieHostConfig;
        private readonly IList<GasStationSettings> _gasStations;
        private readonly ILoggerFactory _loggerFactory;

        public HomieDeviceHost DeviceHost { get; private set; }
        public Device RootDevice { get; private set; }
        public bool IsStarted { get { return (DeviceHost == null) ? false : DeviceHost.IsStarted; } }

        public Dictionary<string, FloatProperty> MappedProperties { get; } = new();

        public HomieEnvironmentBuilder(string deviceId, string deviceName, HomieMqttServerConfiguration homieHostConfig, IList<GasStationSettings> gasStations, ILoggerFactory loggerFactory)
        {
            _deviceId = deviceId;
            _deviceName = deviceName;
            _homieHostConfig = homieHostConfig;
            _gasStations = gasStations;
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
            AddAllPriceProperties();
            AddUpdateAllCommandNode();
        }

        private void AddAllPriceProperties()
        {
            foreach (var station in _gasStations)
            {
                var id = station.StationId.ToLowerInvariant();
                var node = RootDevice.AddNode(id, "GasStation",station.Name);
                MappedProperties.Add(id + "-diesel", node.AddFloatProperty("diesel", "Diesel", unit: "€"));
                MappedProperties.Add(id + "-supere5", node.AddFloatProperty("supere5", "Super E5", unit: "€"));
                MappedProperties.Add(id + "-supere10", node.AddFloatProperty("supere10", "Super E10", unit: "€"));
            }
        }

        private void AddUpdateAllCommandNode()
        {
            RootDevice
                .AddNode("update", "command", "Update anfordern")
                .AddBooleanProperty("all", "UpdateAll Command", "", true)
                    .PropertyCommandReceived += UpdateAllReceived;

        }

        private void CreateHost()
        {
            DeviceHost = new HomieDeviceHost(RootDevice, _homieHostConfig, _loggerFactory.CreateLogger<HomieDeviceHost>());
        }

        private void UpdateAllReceived(DateTime timestamp, string newvalue)
        {
        }
    }
}