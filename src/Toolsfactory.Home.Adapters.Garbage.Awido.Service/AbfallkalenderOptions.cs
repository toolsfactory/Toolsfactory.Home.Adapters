using System;
using System.Collections.Generic;
using Toolsfactory.Protocols.Homie.Devices;

namespace Toolsfactory.Home.Adapters.Garbage.Awido.Service
{
    public class AbfallkalenderOptions
    {
        public UInt16 StartupDelaySeconds { get; set; }
        public UInt16 UpdateDelaySeconds { get; set; }
        public string HomieDeviceIdentifier { get; set; }
        public string HomieDeviceName { get; set; }
        public UInt16 TimerIntervalSeconds { get; set; } = 60 * 60 * 2;
        public CalendarLoaderOptions CalendarLoader { get; set; }
        public Dictionary<string,string> CalendarCategoriesMapping { get; set; }
    }
}
