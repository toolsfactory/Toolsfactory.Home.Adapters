using System.Collections.Generic;

namespace Toolsfactory.Home.Adapters.Gasprices.Tankerkoenig.DTO
{
    public class PriceListDto : RequestStatusDto
    {
        public IDictionary<string, PriceDto> Prices { get; set; }
    }
}
