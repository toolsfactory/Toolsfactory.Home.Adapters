﻿namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig.DTO
{
    public class DetailedStationDto : StationDto
    {
        public OpeningTimeDto[] OpeningTimes { get; set; }
        public string[] Overrides { get; set; }
        public bool WholeDay { get; set; }
        public string State { get; set; }
    }
}
