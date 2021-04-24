using System.Threading.Tasks;
using Sidekick.Infrastructure.PoeNinja.Models;

namespace Sidekick.Infrastructure.PoeNinja
{
    public interface IPoeNinjaClient
    {
        bool IsSupportingCurrentLanguage { get; }

        Task<PoeNinjaQueryResult<PoeNinjaItem>> FetchItems(ItemType itemType);

        Task<PoeNinjaQueryResult<PoeNinjaCurrency>> FetchCurrencies(CurrencyType currency);
    }
}
