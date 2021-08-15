using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Apis.PoeNinja.Repository.Models;
using Sidekick.Common.Game.Items;

namespace Sidekick.Apis.PoeNinja.Repository
{
    public interface IPoeNinjaRepository
    {
        /// <summary>
        /// Find a NinjaPrice by the name (english or translated) of the item
        /// </summary>
        /// <param name="item">The item to find the price for.</param>
        /// <returns>The NinjaPrice information, or null if it was not found.</returns>
        Task<NinjaPrice> Find(Item item);

        /// <summary>
        /// Save translations in the database
        /// </summary>
        /// <param name="translations">The translations to save in the database.</param>
        /// <returns></returns>
        Task SaveTranslations(List<NinjaTranslation> translations);

        /// <summary>
        /// Save ninja prices in the database
        /// </summary>
        /// <param name="prices">The prices to save in the database.</param>
        /// <returns></returns>
        Task SavePrices(List<NinjaPrice> prices);

        /// <summary>
        /// Clears the data stored in the database
        /// </summary>
        /// <returns></returns>
        Task Clear();
    }
}
