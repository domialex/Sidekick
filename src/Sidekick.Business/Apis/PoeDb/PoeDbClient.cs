using System;
using Sidekick.Business.Languages.Client;
using Sidekick.Core.Loggers;
using Sidekick.Platforms;

namespace Sidekick.Business.Apis.PoeDb
{
    public class PoeDbClient : IPoeDbClient
    {
        private const string PoeDbBaseUri = "https://poedb.tw/";
        private const string SubUrlUnique = "unique.php?n=";
        private const string SubUrlGem = "gem.php?n=";
        private const string SubUrlItem = "item.php?n=";
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly INativeBrowser nativeBrowser;

        public PoeDbClient(ILogger logger,
            ILanguageProvider languageProvider,
            INativeBrowser nativeBrowser)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.nativeBrowser = nativeBrowser;
        }

        public void Open(Parsers.Models.Item item)
        {
            if (item == null)
            {
                return;
            }

            if (languageProvider.Current.Name != languageProvider.DefaultLanguage)        // Only English for now
            {
                return;
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                logger.Log("Failed to open PoeDb for item", LogState.Error);
                return;
            }

            nativeBrowser.Open(CreateUri(item));
        }

        private Uri CreateUri(Parsers.Models.Item item)
        {
            string subUrl;

            if (item.Rarity == languageProvider.Language.RarityUnique)
            {
                subUrl = SubUrlUnique;
            }
            else if (item.Rarity == languageProvider.Language.RarityGem)
            {
                subUrl = SubUrlGem;
            }
            else
            {
                subUrl = SubUrlItem;
            }

            var searchLink = item.Rarity == languageProvider.Language.RarityUnique ? item.Name : item.Type;
            string wikiLink = subUrl + searchLink.Replace(" ", "+");
            return new Uri(PoeDbBaseUri + wikiLink);
        }
    }
}
