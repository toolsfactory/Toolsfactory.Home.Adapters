namespace Toolsfactory.Home.Adapters.Powermeter.D0
{
    public class PowermeterConfig
    {
        public string SerialPort { get; set; }
        public int DelaySec { get; set; }
        public string? HomieDeviceIdentifier { get; set; }
        public string? HomieDeviceName { get; set; }
    }
}
