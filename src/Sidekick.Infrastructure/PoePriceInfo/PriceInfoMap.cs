using AutoMapper;
using Sidekick.Domain.Apis.PoePriceInfo.Models;
using Sidekick.Infrastructure.PoePriceInfo.Models;

namespace Sidekick.Infrastructure.PoePriceInfo
{
    public class PriceInfoMap : Profile
    {
        public PriceInfoMap()
        {
            CreateMap<PriceInfoResult, PricePrediction>();
        }
    }
}
