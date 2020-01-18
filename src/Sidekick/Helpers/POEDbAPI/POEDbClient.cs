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

            if (Legacy.LanguageProvider.Current != LanguageEnum.English)        // Only English for now
            {
                return;
            }

            if (item.Rarity == Legacy.LanguageProvider.Language.RarityRare || item.Rarity == Legacy.LanguageProvider.Language.RarityMagic)
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

            string wikiLink = subUrl + item.Name.Replace(" ", "+");
            return new Uri(PoeDbBaseUri, wikiLink);
        }
    }
}
