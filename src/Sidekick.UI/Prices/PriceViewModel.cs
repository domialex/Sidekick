using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Languages;
using Sidekick.Business.Parsers;
using Sidekick.Business.Parsers.Models;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Localization.Prices;
using Sidekick.UI.Items;

namespace Sidekick.UI.Prices
{
    [AddINotifyPropertyChangedInterface]
    public class PriceViewModel : IPriceViewModel
    {
        private readonly ITradeSearchService tradeSearchService;
        private readonly IPoeNinjaCache poeNinjaCache;
        private readonly IStaticDataService staticDataService;
        private readonly ILanguageProvider languageProvider;
        private readonly IPoePriceInfoClient poePriceInfoClient;
        private readonly INativeClipboard nativeClipboard;
        private readonly IItemParser itemParser;
        private readonly SidekickSettings settings;

        public PriceViewModel(
            ITradeSearchService tradeSearchService,
            IPoeNinjaCache poeNinjaCache,
            IStaticDataService staticDataService,
            ILanguageProvider languageProvider,
            IPoePriceInfoClient poePriceInfoClient,
            INativeClipboard nativeClipboard,
            IItemParser itemParser,
            SidekickSettings settings)
        {
            this.tradeSearchService = tradeSearchService;
            this.poeNinjaCache = poeNinjaCache;
            this.staticDataService = staticDataService;
            this.languageProvider = languageProvider;
            this.poePriceInfoClient = poePriceInfoClient;
            this.nativeClipboard = nativeClipboard;
            this.itemParser = itemParser;
            this.settings = settings;
            Task.Run(Initialize);
        }

        public Business.Parsers.Models.Item Item { get; private set; }

        public string ItemColor => Item?.GetColor();

        public ObservableCollection<PriceItem> Results { get; private set; }

        private FetchResult<Result> QueryResult { get; set; }

        public Uri Uri => QueryResult?.Uri;

        public bool IsError { get; private set; }
        public bool IsNotError => !IsError;

        public bool IsFetching { get; private set; }
        public bool IsFetched => !IsFetching;

        public bool IsCurrency { get; private set; }

        private async Task Initialize()
        {
            Item = await itemParser.ParseItem(nativeClipboard.LastCopiedText, false);
            Results = null;

            if (Item == null)
            {
                IsError = true;
                return;
            }

            IsFetching = true;
            QueryResult = await tradeSearchService.GetListings(Item);
            IsFetching = false;
            if (QueryResult.Result.Any())
            {
                Append(QueryResult.Result);
                IsCurrency = Results.FirstOrDefault()?.Item?.Item?.Rarity == Rarity.Currency;
            }

            UpdateCountString();
            GetPoeNinjaPrice();

            if (settings.EnablePricePrediction)
            {
                _ = GetPredictionPrice();
            }
        }

        public async Task LoadMoreData()
        {
            if (IsFetching || Results.Count >= 100)
            {
                return;
            }

            var page = (int)Math.Ceiling(Results.Count / 10d);
            IsFetching = true;
            QueryResult = await tradeSearchService.GetListingsForSubsequentPages(Item, page);
            IsFetching = false;
            if (QueryResult.Result.Any())
            {
                Append(QueryResult.Result);
            }

            UpdateCountString();
        }

        private void Append(List<Result> results)
        {
            var items = new List<PriceItem>();

            foreach (var result in results)
            {
                items.Add(new PriceItem(result)
                {
                    ImageUrl = new Uri(
                        languageProvider.Language.PoeCdnBaseUrl,
                        staticDataService.GetImage(result.Listing.Price.Currency)
                    ).AbsoluteUri,
                });
            }

            if (Results == null)
            {
                Results = new ObservableCollection<PriceItem>(items);
            }
            else
            {
                items.ForEach(x => Results.Add(x));
            }
        }

        public bool IsPredicted => !string.IsNullOrEmpty(PredictionText);
        public string PredictionText { get; private set; }
        private async Task GetPredictionPrice()
        {
            PredictionText = string.Empty;

            if (!IsPoeNinja)
            {
                var result = await poePriceInfoClient.GetItemPricePrediction(Item.ItemText);
                if (result?.ErrorCode != 0)
                {
                    return;
                }

                PredictionText = string.Format(
                    PriceResources.PredictionString,
                    $"{result.Min?.ToString("N1")}-{result.Max?.ToString("N1")} {result.Currency}",
                    result.ConfidenceScore.ToString("N1"));
            }
        }

        public bool IsPoeNinja => !string.IsNullOrEmpty(PoeNinjaText);
        public string PoeNinjaText { get; private set; }
        private void GetPoeNinjaPrice()
        {
            PoeNinjaText = string.Empty;

            var poeNinjaItem = poeNinjaCache.GetItem(Item);
            if (poeNinjaItem != null)
            {
                PoeNinjaText = string.Format(
                    PriceResources.PoeNinjaString,
                    poeNinjaItem.ChaosValue,
                    poeNinjaCache.LastRefreshTimestamp.Value.ToString("HH:mm"));
            }
        }

        public string CountString { get; private set; }
        private void UpdateCountString()
        {
            CountString = string.Format(PriceResources.CountString, Results?.Count ?? 0, QueryResult?.Total.ToString() ?? "?");
        }

        public bool HasPreviewItem { get; private set; }
        public PriceItem PreviewItem { get; private set; }
        public void Preview(PriceItem selectedItem)
        {
            PreviewItem = selectedItem;
            HasPreviewItem = PreviewItem != null;
        }
    }
}
