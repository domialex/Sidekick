using AutoMapper;
using Sidekick.Domain.Apis.PoePriceInfo.Models;
using Sidekick.Infrastructure.PoeNinja.Models;

namespace Sidekick.Infrastructure.PoeNinja
{
    public class PoeNinjaMap : Profile
    {
        public PoeNinjaMap()
        {
            CreateMap<PoeNinjaQueryResult<PoeNinjaItem>, PricePrediction>();
        }
    }
}
