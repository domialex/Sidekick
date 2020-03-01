using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoeNinja.Models;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Categories;
using Sidekick.Business.Languages;
using Sidekick.Business.Parsers;
using Sidekick.Business.Trades;
using Sidekick.Business.Trades.Results;
using Sidekick.Core.Natives;
using Sidekick.Localization.Prices;
using Sidekick.UI.Items;

namespace Sidekick.UI.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceViewModel : IPriceViewModel
    {
        private readonly ITradeClient tradeClient;
        private readonly IPoeNinjaCache poeNinjaCache;
        private readonly IStaticItemCategoryService staticItemCategoryService;
        private readonly ILanguageProvider languageProvider;
        private readonly IPoePriceInfoClient poePriceInfoClient;
        private readonly INativeClipboard nativeClipboard;
        private readonly IItemParser itemParser;

        public PriceViewModel(
            ITradeClient tradeClient,
            IPoeNinjaCache poeNinjaCache,
            IStaticItemCategoryService staticItemCategoryService,
            ILanguageProvider languageProvider,
            IPoePriceInfoClient poePriceInfoClient,
            INativeClipboard nativeClipboard,
            IItemParser itemParser)
        {
            this.tradeClient = tradeClient;
            this.poeNinjaCache = poeNinjaCache;
            this.staticItemCategoryService = staticItemCategoryService;
            this.languageProvider = languageProvider;
            this.poePriceInfoClient = poePriceInfoClient;
            this.nativeClipboard = nativeClipboard;
            this.itemParser = itemParser;

            Task.Run(Initialize);
        }

        public Business.Parsers.Models.Item Item { get; private set; }

        public string ItemColor => Item?.GetColor();

        public ObservableCollection<PriceItem> Results { get; private set; }

        private QueryResult<SearchResult> QueryResult { get; set; }

        public bool IsFetching { get; private set; }

        public bool IsFetched => !IsFetching;

        private async Task Initialize()
        {
            Item = await itemParser.ParseItem(nativeClipboard.LastCopiedText);
            Results = null;

            if (Item == null)
            {
                return;
            }

            IsFetching = true;
            QueryResult = await tradeClient.GetListings(Item);
            IsFetching = false;
            if (QueryResult.Result.Any())
            {
                Append(QueryResult.Result);
            }

            UpdateCountString();
            GetPoeNinjaPrice();
            _ = GetPredictionPrice();
        }

        public async Task LoadMoreData()
        {
            if (IsFetching || Results.Count >= 100)
            {
                return;
            }

            var page = (int)Math.Ceiling(Results.Count / 10d);
            IsFetching = true;
            QueryResult = await tradeClient.GetListingsForSubsequentPages(Item, page);
            IsFetching = false;
            if (QueryResult.Result.Any())
            {
                Append(QueryResult.Result);
            }

            UpdateCountString();
        }

        private void Append(List<SearchResult> results)
        {
            if (Results == null)
            {
                Results = new ObservableCollection<PriceItem>();
            }

            foreach (var result in results)
            {
                staticItemCategoryService.CurrencyUrls.TryGetValue(result.Listing.Price.Currency, out var url);

                Results.Add(new PriceItem(result)
                {
                    CurrencyUrl = $"{languageProvider.Language.PoeCdnBaseUrl}{url}"
                });
            }
        }

        public string PredictionText { get; private set; }

        private async Task GetPredictionPrice()
        {
            PredictionText = string.Empty;

            if (Item.Rarity == Business.Parsers.Models.Rarity.Rare && Item.IsIdentified)
            {
                var result = await poePriceInfoClient.GetItemPricePrediction(Item.ItemText);
                if (result?.ErrorCode != 0)
                {
                    return;
                }

                PredictionText = $"{result.Min?.ToString("F")}-{result.Max?.ToString("F")} {result.Currency} ({result.ConfidenceScore.ToString("N1")}%)";
            }
        }

        public PoeNinjaItem PoeNinjaItem { get; private set; }
        public DateTime? PoeNinjaLastRefreshTimestamp { get; private set; }

        private void GetPoeNinjaPrice()
        {
            PoeNinjaItem = null;
            PoeNinjaLastRefreshTimestamp = null;

            var poeNinjaItem = poeNinjaCache.GetItem(Item);
            if (poeNinjaItem != null)
            {
                PoeNinjaItem = poeNinjaItem;
                PoeNinjaLastRefreshTimestamp = poeNinjaCache.LastRefreshTimestamp;
            }
        }

        public string CountString { get; private set; }
        private void UpdateCountString()
        {
            CountString = string.Format(PriceResources.CountString, Results?.Count ?? 0, QueryResult?.Total.ToString() ?? "?");
        }
    }
}
