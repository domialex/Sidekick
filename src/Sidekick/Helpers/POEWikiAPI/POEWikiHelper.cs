using Sidekick.Core.Loggers;
using System;
using System.Diagnostics;

namespace Sidekick.Helpers.POEWikiAPI
{
    public static class POEWikiHelper
    {
        private static readonly Uri WIKI_BASE_URI = new Uri("https://pathofexile.gamepedia.com/");

        /// <summary>
        /// Attempts to generate and open the wiki link for the given item
        /// </summary>
        public static void Open(Business.Parsers.Models.Item item)
        {
            if (item == null)
                return;

            // only available for english portal
            if (Legacy.LanguageProvider.Current.Name != Legacy.LanguageProvider.DefaultLanguage)
                return;

            // Most items will open the basetype wiki link.
            // Does not work for unique items that are not identified.
            if (string.IsNullOrEmpty(item.Name))
            {
                Legacy.Logger.Log("Failed to open the wiki for the specified item.", LogState.Error);
                return;
            }

            var uri = CreateItemWikiLink(item).ToString();
            Legacy.Logger.Log(string.Format("Opening in browser: {0}", uri));
            Process.Start(uri);
        }

        /// <summary>
        /// Creates and returns a URI link for the given item in a format matching that of the poe gamepedia website
        /// Only works with items that are not rare or magic
        /// </summary>
        private static Uri CreateItemWikiLink(Business.Parsers.Models.Item item)
        {
            // determine search link, so wiki can be opened for any item
            var searchLink = item.Rarity == Legacy.LanguageProvider.Language.RarityUnique ? item.Name : item.Type;
            // replace space encodes with '_' to match the link layout of the poe wiki and then url encode it
            string itemLink = System.Net.WebUtility.UrlEncode(searchLink.Replace(" ", "_"));
            return new Uri(WIKI_BASE_URI, itemLink);
        }
    }
}
