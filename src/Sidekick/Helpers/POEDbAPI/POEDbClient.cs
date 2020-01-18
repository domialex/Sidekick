using Sidekick.Business.Languages;
using Sidekick.Core.Loggers;
using System;
using System.Diagnostics;

namespace Sidekick.Helpers.POEDbAPI
{
    public static class POEDbClient
    {
        public static readonly Uri PoeDbBaseUri = new Uri("https://poedb.tw");
        public const string SubUrlUnique = "unique.php?n=";
        public const string SubUrlGem = "gem.php?n=";
        public const string SubUrlItem = "item.php?n=";

        public static void Open(Sidekick.Business.Parsers.Models.Item item)
        {
            if (item == null)
            {
                return;
            }

            // Only English for now.
            if (Legacy.LanguageProvider.Current != LanguageEnum.English)
            {
                return;
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                Legacy.Logger.Log("Failed to open PoeDb for item", LogState.Error);
                return;
            }

            var url = CreateUri(item).ToString();
            Legacy.Logger.Log(string.Format("Opening in browser: {0}", url));
            Process.Start(url);
        }

        private static Uri CreateUri(Sidekick.Business.Parsers.Models.Item item)
        {
            string subUrl;

            if (item.Rarity == Legacy.LanguageProvider.Language.RarityUnique)
            {
                subUrl = SubUrlUnique;
            }
            else if (item.Rarity == Legacy.LanguageProvider.Language.RarityGem)
            {
                subUrl = SubUrlGem;
            }
            else
            {
                subUrl = SubUrlItem;
            }

            var searchLink = item.Rarity == Legacy.LanguageProvider.Language.RarityUnique ? item.Name : item.Type;
            string wikiLink = subUrl + searchLink.Replace(" ", "+");
            return new Uri(PoeDbBaseUri, wikiLink);
        }
    }
}
