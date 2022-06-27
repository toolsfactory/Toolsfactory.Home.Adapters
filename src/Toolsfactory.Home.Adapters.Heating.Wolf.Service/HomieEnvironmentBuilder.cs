using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Tiveria.Common.Extensions;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Heating.Wolf.Service
{
    public class HomieEnvironmentBuilder
    {
        private const string FormatPropertyKey = "nextformat";

        private readonly string _deviceId;
        private readonly string _deviceName;
        private readonly IReadOnlyDictionary<string, string> _categoriesMapping;
        private readonly HomieHostConfiguration _homieHostConfig;
        private readonly ILoggerFactory _loggerFactory;
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public HomieDeviceHost DeviceHost { get; private set; }
        public Device RootDevice { get; private set; }
        public bool IsStarted { get { return (DeviceHost == null) ? false : DeviceHost.IsStarted; } }

        public HomieEnvironmentBuilder(string deviceId, string deviceName, IReadOnlyDictionary<string, string> categoriesMapping, HomieHostConfiguration homieHostConfig, ILoggerFactory loggerFactory)
        {
            _deviceId = deviceId;
            _deviceName = deviceName;
            _categoriesMapping = categoriesMapping;
            _homieHostConfig = homieHostConfig;
            _loggerFactory = loggerFactory;
            CreateDevice();
            CreateHost();
        }

        public async Task StartAsync()
        {
            await DeviceHost.StartAsync();
            UpdateItems();
        }

        public async Task StopAsync()
        {
            await DeviceHost.StopAsync();
        }

        public void UpdateItems()
        {
            _semaphoreSlim.Wait();
            try
            {

                if (IsStarted)
                    UpdateAll();
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void CreateDevice()
        {
            RootDevice = new Device(_deviceId, _deviceName);
            AddAnyNodeAndProperties();
            AddStatusNodeAndProperties();
            AddUpdateAllCommandNode();
        }

        private void AddAnyNodeAndProperties()
        {
        }

        private void AddUpdateAllCommandNode()
        {
            RootDevice
                .AddNode("update", "command", "Update anfordern")
                .AddBooleanProperty("all", "UpdateAll Command", "", true)
                    .PropertyCommandReceived += UpdateAllReceived;

        }

        private void AddStatusNodeAndProperties()
        {
        }

        private void CreateHost()
        {
            DeviceHost = new HomieDeviceHost(RootDevice, _homieHostConfig, _loggerFactory.CreateLogger<HomieDeviceHost>());
        }

        private void UpdateAllReceived(DateTime timestamp, string newvalue)
        {
            UpdateAll();
        }

        private void UpdateAll()
        {
        }
    }
}

