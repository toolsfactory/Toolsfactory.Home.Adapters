using System.Collections.Generic;
using Toolsfactory.Home.Adapters.Gasprices;

namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig.DTO
{
    internal static class DtoExtensions
    {
        internal static GasStationDetails MapToBusiness(this StationDto source)
        {
            var Gasprices = new Dictionary<GasType, float?>(3);
            Gasprices.Add(GasType.Diesel, source.Diesel);
            Gasprices.Add(GasType.E5, source.E5);
            Gasprices.Add(GasType.E10, source.E10);
            return new GasStationDetails()
            {
                ID = source.Id,
                Brand = source.Brand,
                Name = source.Name,
                Open = source.IsOpen,
                Distance = source.Dist,
                Location = new Location(source.Lat, source.Lng, source.City, source.Street, source.HouseNumber, source.PostCode),
                Gasprices = Gasprices
            };
        }

        internal static Dictionary<GasType, float?> MapToBusiness(this PriceDto source)
        {
            var Gasprices = new Dictionary<GasType, float?>(3);
            Gasprices.Add(GasType.Diesel, source.Diesel);
            Gasprices.Add(GasType.E5, source.E5);
            Gasprices.Add(GasType.E10, source.E10);
            return Gasprices;
        }
    }

}
