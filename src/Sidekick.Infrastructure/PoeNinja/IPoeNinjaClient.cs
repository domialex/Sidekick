using System.Threading.Tasks;
using Sidekick.Infrastructure.PoeNinja.Models;

namespace Sidekick.Infrastructure.PoeNinja
{
    public interface IPoeNinjaClient
    {
        bool IsSupportingCurrentLanguage { get; }

        Task<PoeNinjaQueryResult<PoeNinjaItem>> QueryItem(string leagueId, ItemType itemType);

        Task<PoeNinjaQueryResult<PoeNinjaCurrency>> QueryItem(string leagueId, CurrencyType currency);
    }
}
