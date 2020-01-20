using Sidekick.Business.Apis.PoeNinja.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Business.Apis.PoeNinja
{
    public interface IPoeNinjaClient
    {
        Task<PoeNinjaQueryResult<PoeNinjaItem>> GetItemOverview(string league, ItemType itemType);

        Task<PoeNinjaQueryResult<PoeNinjaCurrency>> GetCurrencyOverview(string league, CurrencyType currency);
    }
}
