using Sidekick.Helpers.POETradeAPI.Models.TradeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POEPriceInfoAPI
{
    public static class PriceInfoClient
    {
        public const string PoePricesBaseUrl = "https://www.poeprices.inof/api";
        public static League SelectedLeague;

        public static void GetItemPricePrediction(string itemText)
        {
            var encodedItem = EncodeItemToBase64(itemText);
            var league = SelectedLeague.Id;
            var requestUrl = GenerateRequestUrl(encodedItem, league);
            var doc = new HtmlAgilityPack.HtmlWeb();
            var html = doc.Load(requestUrl);
        }

        private static string EncodeItemToBase64(string itemText)
        {
            var bytes = Encoding.UTF8.GetBytes(itemText);
            return Convert.ToBase64String(bytes);
        }

        private static string GenerateRequestUrl(string encodedItem, string selectedLeague)
        {
            return PoePricesBaseUrl + "?l=" + selectedLeague + "&i=" + encodedItem;
        }
    }
}
