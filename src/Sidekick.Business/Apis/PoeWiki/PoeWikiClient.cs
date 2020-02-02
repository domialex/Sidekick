using System;
using Sidekick.Business.Languages;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;

namespace Sidekick.Business.Apis.PoeWiki
{
    public class PoeWikiClient : IPoeWikiClient
    {
        private const string WIKI_BASE_URI = "https://pathofexile.gamepedia.com/";
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly INativeBrowser nativeBrowser;

        public PoeWikiClient(ILogger logger,
            ILanguageProvider languageProvider,
            INativeBrowser nativeBrowser)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.nativeBrowser = nativeBrowser;
        }

        /// <summary>
        /// Attempts to generate and open the wiki link for the given item
        /// </summary>
        public void Open(Parsers.Models.Item item)
        {
            if (item == null)
            {
                return;
            }

            // only available for english portal
            if (!languageProvider.IsEnglish)
            {
                return;
            }

            // Most items will open the basetype wiki link.
            // Does not work for unique items that are not identified.
            if (string.IsNullOrEmpty(item.Name))
            {
                logger.Log("Failed to open the wiki for the specified item.", LogState.Error);
                return;
            }

            nativeBrowser.Open(CreateItemWikiLink(item));
        }

        /// <summary>
        /// Creates and returns a URI link for the given item in a format matching that of the poe gamepedia website
        /// Only works with items that are not rare or magic
        /// </summary>
        private Uri CreateItemWikiLink(Parsers.Models.Item item)
        {
            // determine search link, so wiki can be opened for any item
            var searchLink = item.Rarity == languageProvider.Language.RarityUnique ? item.Name : item.Type;
            // replace space encodes with '_' to match the link layout of the poe wiki and then url encode it
            var itemLink = System.Net.WebUtility.UrlEncode(searchLink.Replace(" ", "_"));
            return new Uri(WIKI_BASE_URI + itemLink);
        }
    }
}
