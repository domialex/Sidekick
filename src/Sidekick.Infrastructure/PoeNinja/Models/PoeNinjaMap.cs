using AutoMapper;
using Sidekick.Domain.Apis.PoePriceInfo.Models;

namespace Sidekick.Infrastructure.PoeNinja.Models
{
    public class PoeNinjaMap : Profile
    {
        public PoeNinjaMap()
        {
            CreateMap<PoeNinjaQueryResult<PoeNinjaItem>, PricePrediction>();
        }
    }
}
