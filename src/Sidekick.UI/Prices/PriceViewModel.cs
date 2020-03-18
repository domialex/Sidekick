using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PropertyChanged;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Static;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Business.Apis.Poe.Trade.Search.Results;
using Sidekick.Business.Apis.PoeNinja;
using Sidekick.Business.Apis.PoePriceInfo.Models;
using Sidekick.Business.Languages;
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
        private readonly IParserService parserService;
        private readonly SidekickSettings settings;
        private readonly IStatDataService statDataService;

        public PriceViewModel(
            ITradeSearchService tradeSearchService,
            IPoeNinjaCache poeNinjaCache,
            IStaticDataService staticDataService,
            ILanguageProvider languageProvider,
            IPoePriceInfoClient poePriceInfoClient,
            INativeClipboard nativeClipboard,
            IParserService parserService,
            SidekickSettings settings,
            IStatDataService statDataService)
        {
            this.tradeSearchService = tradeSearchService;
            this.poeNinjaCache = poeNinjaCache;
            this.staticDataService = staticDataService;
            this.languageProvider = languageProvider;
            this.poePriceInfoClient = poePriceInfoClient;
            this.nativeClipboard = nativeClipboard;
            this.parserService = parserService;
            this.settings = settings;
            this.statDataService = statDataService;
            Task.Run(Initialize);
        }

        public ParsedItem Item { get; private set; }

        public string ItemColor => Item?.GetColor();

        public ObservableCollection<PriceItem> Results { get; private set; }

        private FetchResult<Result> QueryResult { get; set; }

        public Uri Uri => QueryResult?.Uri;

        public bool IsError { get; private set; }
        public bool IsNotError => !IsError;

        public bool IsFetching { get; private set; }
        public bool IsFetched => !IsFetching;

        public bool IsCurrency { get; private set; }

        public ObservableCollection<PriceModifierCategory> Modifiers { get; set; }

        private async Task Initialize()
        {
            Item = await parserService.ParseItem(nativeClipboard.LastCopiedText);

            InitializeMods(Item.Extended.Mods.Explicit);
            InitializeMods(Item.Extended.Mods.Implicit);
            InitializeMods(Item.Extended.Mods.Crafted);
            InitializeMods(Item.Extended.Mods.Enchant);

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

        private void InitializeMods(List<Mod> mods)
        {
            if (mods.Count == 0)
            {
                return;
            }

            PriceModifierCategory category = null;

            var magnitudes = mods
                .SelectMany(x => x.Magnitudes)
                .GroupBy(x => x.Hash)
                .Select(x => new
                {
                    Definition = statDataService.GetById(x.First().Hash),
                    Magnitudes = x
                })
                .ToList();

            foreach (var magnitude in magnitudes)
            {
                if (category == null)
                {
                    category = new PriceModifierCategory()
                    {
                        Label = magnitude.Definition.Category
                    };
                }

                category.Modifiers.Add(new PriceModifier()
                {
                    Text = magnitude.Definition.Text,
                    Enabled = false,
                });
            }

            if (Modifiers == null)
            {
                Modifiers = new ObservableCollection<PriceModifierCategory>();
            }

            if (category != null)
            {
                Modifiers.Add(category);
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
