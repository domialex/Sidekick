using AutoMapper;
using Sidekick.Domain.Apis.PoePriceInfo.Models;

namespace Sidekick.Infrastructure.PoePriceInfo.Models
{
    public class PriceInfoMap : Profile
    {
        public PriceInfoMap()
        {
            CreateMap<PriceInfoResult, PricePrediction>();
        }
    }
}
