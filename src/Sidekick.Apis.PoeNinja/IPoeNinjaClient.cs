using System.Threading.Tasks;
using Sidekick.Apis.PoeNinja.Repository.Models;
using Sidekick.Common.Game.Items;

namespace Sidekick.Apis.PoeNinja
{
    public interface IPoeNinjaClient
    {
        bool IsSupportingCurrentLanguage { get; }

        Task<NinjaPrice> GetPriceInfo(Item item);
    }
}
