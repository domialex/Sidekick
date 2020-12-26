using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Domain.Apis.PoeNinja.Models;

namespace Sidekick.Domain.Apis.PoeNinja
{
    public interface IPoeNinjaRepository
    {
        /// <summary>
        /// Find a NinjaPrice by the name (english or translated) of the item
        /// </summary>
        /// <param name="name">The name of the item. It can be in any language</param>
        /// <param name="corrupted">If the item is corrupted or not</param>
        /// <param name="mapTier">If it is a map, indicates the tier of the map</param>
        /// <param name="gemLevel">If it is a gem, indicates the level of the gem</param>
        /// <returns>The NinjaPrice information, or null if it was not found.</returns>
        Task<NinjaPrice> Find(string name, bool corrupted, int mapTier, int gemLevel);

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
