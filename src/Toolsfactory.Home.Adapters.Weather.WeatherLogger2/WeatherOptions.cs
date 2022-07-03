using System;

namespace Toolsfactory.Home.Adapters.Weather.WeatherLogger2
{
    public class WeatherOptions 
    {
        public UInt16 StartupDelaySeconds { get; set; }
        public LocalServerOptions LocalServer { get; set; }
        public string? HomieDeviceIdentifier { get; set; }
        public string? HomieDeviceName { get; set; }
        public List<Node>? HomieDeviceNodes { get; set; }
    }

    public class LocalServerOptions
    {
        public UInt16 Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class WeatherItem
    {
        public string SourceName { get; set; }
        public string SourceUnit { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
    public class Node
    {
        public string? Name { get; set; }
        public List<WeatherItem>? Properties { get; set; }
    }

}
