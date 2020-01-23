using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeNinja.Models;

namespace Sidekick.Business.Apis.PoeNinja
{
    public interface IPoeNinjaClient
    {
        Task<PoeNinjaQueryResult<PoeNinjaItem>> QueryItem(string leagueId, ItemType itemType);

        Task<PoeNinjaQueryResult<PoeNinjaCurrency>> QueryItem(string leagueId, CurrencyType currency);
    }
}
