using AutoMapper;
using Sidekick.Apis.PoeNinja.Models;
using Sidekick.Domain.Apis.PoePriceInfo.Models;

namespace Sidekick.Apis.PoeNinja
{
    public class PoeNinjaMap : Profile
    {
        public PoeNinjaMap()
        {
            CreateMap<PoeNinjaQueryResult<PoeNinjaItem>, PricePrediction>();
        }
    }
}
