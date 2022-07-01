using System;
using System.Collections.Generic;

namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig
{
    public class GaspricesOptions
    {
        public UInt16 StartupDelaySeconds { get; set; }
        public UInt16 UpdateDelaySeconds { get; set; }
        public string? HomieDeviceIdentifier { get; set; }
        public string? HomieDeviceName { get; set; }
        public TankerkoenigOptions Tankerkoenig { get; set; }
    }

    public class TankerkoenigOptions : TankerkoenigManagerOptions
    {
        public IList<GasStationSettings> GasStations { get; set; }
    }

    public class GasStationSettings
    {
        public string StationId { get; set; }
        public string Name { get; set; }
        public string DieselItem { get; set; }
        public string E5Item { get; set; }
        public string E10Item { get; set; }
    }
}