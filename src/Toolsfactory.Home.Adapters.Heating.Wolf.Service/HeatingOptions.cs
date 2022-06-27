using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Heating.Wolf.Service
{
    public class HeatingOptions
    {
        public UInt16 StartupDelaySeconds { get; set; }
        public LocalServerOptions LocalServer { get; set; }
        public string HomieDeviceIdentifier { get; set; }
        public string HomieDeviceName { get; set; }
        public HomieHostConfiguration HomieHost { get; set; }
    }

    public class LocalServerOptions
    {
        public UInt16 Port { get; set; }
    }
}