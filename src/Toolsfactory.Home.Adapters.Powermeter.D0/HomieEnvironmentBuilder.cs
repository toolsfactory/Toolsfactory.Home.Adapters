using Microsoft.Extensions.Logging;
using Toolsfactory.Protocols.Homie.Devices;
using Toolsfactory.Protocols.Homie.Devices.Properties;

namespace Toolsfactory.Home.Adapters.Powermeter.D0
{
    public class HomieEnvironmentBuilder
    {
        private const string FormatPropertyKey = "nextformat";

        private readonly string _deviceId;
        private readonly string _deviceName;
        private readonly HomieMqttServerConfiguration _homieHostConfig;
        private readonly ILoggerFactory _loggerFactory;

        public HomieDeviceHost? DeviceHost { get; private set; }
        public Device? RootDevice { get; private set; }
        public FloatProperty? TotalConsumptionProperty { get;private set; }
        public FloatProperty? PeriodConsumptionProperty { get; private set; }
        public bool IsStarted { get { return (DeviceHost == null) ? false : DeviceHost.IsStarted; } }


        public HomieEnvironmentBuilder(string deviceId, string deviceName, HomieMqttServerConfiguration homieHostConfig, ILoggerFactory loggerFactory)
        {
            _deviceId = deviceId;
            _deviceName = deviceName;
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
            AddD0Properties();
        }

        private void AddD0Properties()
        {
            var node = RootDevice.AddNode("consumption");
            TotalConsumptionProperty = node.AddFloatProperty("total");
            PeriodConsumptionProperty = node.AddFloatProperty("period");
        }

        private void CreateHost()
        {
            DeviceHost = new HomieDeviceHost(RootDevice, _homieHostConfig, _loggerFactory.CreateLogger<HomieDeviceHost>());
        }

    }
}