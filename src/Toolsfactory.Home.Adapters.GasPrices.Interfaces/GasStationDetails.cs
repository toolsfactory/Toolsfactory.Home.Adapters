using System;
using System.Collections.Generic;

namespace Toolsfactory.Home.Adapters.Gasprices
{
    public class GasStationDetails
    {
        public string ID { get; set; }
        public bool Open { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public Location Location { get; set; }
        public float Distance { get; set; }
        public IReadOnlyDictionary<GasType, float?> Gasprices { get; set; }
    }
 }
