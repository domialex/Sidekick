using Sidekick.Helpers.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POEDbAPI
{
    public static class POEDbClient
    {
        public static readonly Uri PoeDbBaseUri = new Uri("https://poedb.tw");
        public const string SubUrlUnique = "unique.php?n=";
        public const string SubUrlGem = "gem.php?n=";
        public const string SubUrlItem = "item.php?n=";

        public static void Open(Item item)
        {
            if(item == null)
            {
                return;
            }

            if(LanguageSettings.CurrentLanguage != Language.English)        // Only English for now
            {
                return;
            }

            if(item.Rarity == LanguageSettings.Provider.RarityRare || item.Rarity == LanguageSettings.Provider.RarityMagic)
            {
                return;
            }
          
            if (string.IsNullOrEmpty(item.Name))
            {
                Logger.Log("Failed to open PoeDb for item", LogState.Error);
                return;
            }

            var url = CreateUri(item).ToString();
            Logger.Log(string.Format("Opening in browser: {0}", url));
            Process.Start(url);
        }

        private static Uri CreateUri(Item item)
        {
            string subUrl;

            if(item.Rarity == LanguageSettings.Provider.RarityUnique)
            {
                subUrl = SubUrlUnique;
            }
            else if(item.Rarity == LanguageSettings.Provider.RarityGem)
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
