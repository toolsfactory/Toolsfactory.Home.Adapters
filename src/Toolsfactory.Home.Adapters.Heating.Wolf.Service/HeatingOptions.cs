using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Heating.Wolf.Service
{
    public class HeatingOptions
    {
        public UInt16 StartupDelaySeconds { get; set; }
        public LocalServerOptions? LocalServer { get; set; }
        public string? HomieDeviceIdentifier { get; set; }
        public string? HomieDeviceName { get; set; }
        public List<Node>? HomieDeviceNodes { get; set; }
    }

    public class LocalServerOptions
    {
        public UInt16 Port { get; set; }
    }

    public class HeatingDataPoint
    {
        public ushort Id { get; set; }
        public string Name { get; set; }
        public string DptName { get; set; }
        public string DptId { get; set; }
        public bool Writeable { get; set; }
    }
        public class Node
    {
        public string? Name { get; set; }
        public List<HeatingDataPoint>? Properties { get; set; }
    }
}