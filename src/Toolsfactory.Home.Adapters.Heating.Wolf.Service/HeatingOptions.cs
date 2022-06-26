namespace Toolsfactory.Home.Adapters.Heating.Wolf.Service
{
    public class HeatingOptions
    {
        public UInt16 StartupDelaySeconds { get; set; }
        public LocalServerOptions LocalServer { get; set; }
        public OHServerOptions OHServer { get; set; }
    }

    public class LocalServerOptions
    {
        public UInt16 Port { get; set; }
    }

    public class OHServerOptions
    {
        public string Host { get; set; }
        public string Basepath { get; set; }
        public string ItemLastUpdate { get; set; }
    }
}