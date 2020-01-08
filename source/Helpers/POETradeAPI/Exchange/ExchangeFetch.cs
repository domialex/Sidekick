using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POETradeAPI.Exchange
{
    public class ExchangeFetch
    {
        public static async Task<ExchangeFetchResponse> MakeRequest(HttpClient http, ExchangePrefetchResponse prefetch)
        {
            var url = "https://www.pathofexile.com/api/trade/fetch/" + string.Join(",", prefetch.Result.Take(20)) + $"?query={prefetch.Id}&exchange";
            var response = await http.GetAsync(url);
            Logger.Log($"ExchangeFetch response status: {response.StatusCode}");
            if (!response.IsSuccessStatusCode) return null;

            var responseText = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ExchangeFetchResponse>(responseText);
        }
    }

    public partial class ExchangeFetchResponse
    {
        public ExchangeFetchResult[] Result { get; set; }
    }

    public partial class ExchangeFetchResult
    {
        public string Id { get; set; }
        public ExchangeFetchResultItem Item { get; set; }
        public ExchangeFetchListing Listing { get; set; }
    }

    public partial class ExchangeFetchResultItem
    {
        public bool Verified { get; set; }
        public long W { get; set; }
        public long H { get; set; }
        public Uri Icon { get; set; }
        public string League { get; set; }
        public string Name { get; set; }
        public string TypeLine { get; set; }
        public bool Identified { get; set; }
        public long Ilvl { get; set; }
        public string Note { get; set; }
        public string[] ExplicitMods { get; set; }
        public string DescrText { get; set; }
        public string[] FlavourText { get; set; }
        public long FrameType { get; set; }
    }

    public partial class ExchangeFetchListing
    {
        public string Method { get; set; }
        public DateTimeOffset Indexed { get; set; }
        public ExchangeFetchStash Stash { get; set; }
        public string Whisper { get; set; }
        public ExchangeFetchAccount Account { get; set; }
        public ExchangeFetchPrice Price { get; set; }
    }

    public partial class ExchangeFetchAccount
    {
        public string Name { get; set; }
        public string LastCharacterName { get; set; }
        public ExchangeFetchOnline Online { get; set; }
        public string Language { get; set; }
    }

    public partial class ExchangeFetchOnline
    {
        public string League { get; set; }
        public string Status { get; set; }
    }

    public partial class ExchangeFetchPrice
    {
        public ExchangeFetchExchange Exchange { get; set; }
        public ExchangeFetchPriceItem Item { get; set; }
    }

    public partial class ExchangeFetchExchange
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
    }

    public partial class ExchangeFetchPriceItem
    {
        public string Currency { get; set; }
        public double Amount { get; set; }
        public long Stock { get; set; }
        public string Id { get; set; }
    }

    public partial class ExchangeFetchStash
    {
        public string Name { get; set; }
        public long X { get; set; }
        public long Y { get; set; }
    }

}
