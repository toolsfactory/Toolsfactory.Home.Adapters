namespace Toolsfactory.Home.Adapters.Powermeter.D0.Service
{
    public class DaemonConfig
    {
        public string SerialPort { get; set; }
        public int DelaySec { get; set; }
        public string? HomieDeviceIdentifier { get; set; }
        public string? HomieDeviceName { get; set; }
    }
}
