using System;
using System.Diagnostics;
using Sidekick.Helpers.Localization;

namespace Sidekick.Helpers.POEWikiAPI
{
    public static class POEWikiHelper
    {
        private static readonly Uri WIKI_BASE_URI = new Uri("https://pathofexile.gamepedia.com/");

        /// <summary>
        /// Attempts to generate and open the wiki link for the given item
        /// </summary>
        public static void Open(Item item)
        {
            if (item == null)
                return;

            // only available for english portal
            if (LanguageSettings.CurrentClientLanguage != Language.English)
                return;

            // Don't handle magic and rare items
            // Normal items will open the basetype wiki link which is acceptable
            // Does not work for unique items that are not identified
            if (item.Rarity == LanguageSettings.Provider.RarityRare || item.Rarity == LanguageSettings.Provider.RarityMagic)
                return;

            if (string.IsNullOrEmpty(item.Name))
            {
                Logger.Log("Item with empty name supplied to wiki fetcher. TypeOf(): " + item.GetType().Name, LogState.Warning);
                return;
            }

            Uri uri = CreateItemWikiLink(item);
            Process.Start(uri.ToString());
        }

        /// <summary>
        /// Creates and returns a URI link for the given item in a format matching that of the poe gamepedia website
        /// Only works with items that are not rare or magic
        /// </summary>
        private static Uri CreateItemWikiLink(Item item)
        {
            // replace space encodes with '_' to match the link layout of the poe wiki and then url encode it
            string itemLink = System.Net.WebUtility.UrlEncode(item.Name.Replace(" ", "_"));
            return new Uri(WIKI_BASE_URI, itemLink);
        }
    }
}
