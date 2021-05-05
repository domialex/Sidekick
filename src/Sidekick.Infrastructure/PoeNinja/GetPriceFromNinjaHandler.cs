using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Apis.PoeNinja;
using Sidekick.Domain.Apis.PoeNinja.Models;
using Sidekick.Domain.Apis.PoeNinja.Queries;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Infrastructure.PoeNinja.Models;

namespace Sidekick.Infrastructure.PoeNinja
{
    public class GetPriceFromNinjaHandler : IQueryHandler<GetPriceFromNinjaQuery, NinjaPrice>
    {
        private readonly IPoeNinjaClient poeNinjaClient;
        private readonly IPoeNinjaRepository repository;

        public GetPriceFromNinjaHandler(
            IPoeNinjaClient poeNinjaClient,
            IPoeNinjaRepository repository)
        {
            this.poeNinjaClient = poeNinjaClient;
            this.repository = repository;
        }

        public async Task<NinjaPrice> Handle(GetPriceFromNinjaQuery request, CancellationToken cancellationToken)
        {
            var cacheResult = await repository.Find(request.Item);

            if (cacheResult != null && cacheResult.LastUpdated.AddHours(4) > DateTimeOffset.Now)
            {
                return cacheResult;
            }

            var fetchItems = new List<Task<PoeNinjaQueryResult<PoeNinjaItem>>>();
            var fetchCurrencies = new List<Task<PoeNinjaQueryResult<PoeNinjaCurrency>>>();

            if (request.Item.Metadata.Category == Category.Currency)
            {
                fetchCurrencies.Add(poeNinjaClient.FetchCurrencies(CurrencyType.Currency));
                fetchCurrencies.Add(poeNinjaClient.FetchCurrencies(CurrencyType.Fragment));

                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Incubator));
                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Oil));
                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Incubator));
                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Scarab));
                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Fossil));
                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Resonator));
                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Essence));
                fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Resonator));
            }
            else if (request.Item.Metadata.Rarity == Rarity.Unique)
            {
                switch (request.Item.Metadata.Category)
                {
                    case Category.Accessory: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.UniqueAccessory)); break;
                    case Category.Armour: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.UniqueArmour)); break;
                    case Category.Flask: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.UniqueFlask)); break;
                    case Category.Jewel: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.UniqueJewel)); break;
                    case Category.Map: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.UniqueMap)); break;
                    case Category.Weapon: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.UniqueWeapon)); break;
                    case Category.ItemisedMonster: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Beast)); break;
                }
            }
            else
            {
                switch (request.Item.Metadata.Category)
                {
                    case Category.DivinationCard: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.DivinationCard)); break;
                    case Category.Map: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Map)); break;
                    case Category.Gem: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.SkillGem)); break;
                    case Category.Prophecy: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Prophecy)); break;
                    case Category.ItemisedMonster: fetchItems.Add(poeNinjaClient.FetchItems(ItemType.Beast)); break;
                }
            }

            if (fetchCurrencies.Count == 0 && fetchItems.Count == 0)
            {
                return null;
            }

            var itemResults = await Task.WhenAll(fetchItems);
            var items = itemResults
                .SelectMany(x => x.Lines)
                .Select(x => new NinjaPrice()
                {
                    Corrupted = x.Corrupted,
                    Price = x.ChaosValue,
                    LastUpdated = DateTimeOffset.Now,
                    Name = x.Name,
                    MapTier = x.MapTier,
                    GemLevel = x.GemLevel,
                })
                .ToList();
            await repository.SavePrices(items);
            await SaveTranslations(itemResults);

            var currencyResults = await Task.WhenAll(fetchCurrencies);
            var currencies = currencyResults
                .SelectMany(x => x.Lines)
                .Where(x => x.Receive != null)
                .Select(x => new NinjaPrice()
                {
                    Corrupted = false,
                    Price = x.Receive.Value,
                    LastUpdated = DateTimeOffset.Now,
                    Name = x.CurrencyTypeName,
                })
                .ToList();
            await repository.SavePrices(currencies);
            await SaveTranslations(currencyResults);

            return await repository.Find(request.Item);
        }

        private async Task SaveTranslations<T>(PoeNinjaQueryResult<T>[] results)
        {
            var translations = results
                .SelectMany(x => x.Language.Translations)
                .Where(y => !y.Value.Contains("."))
                .Distinct()
                .Select(x => new NinjaTranslation()
                {
                    English = x.Key,
                    Translation = x.Value,
                })
                .ToList();
            await repository.SaveTranslations(translations);
        }
    }
}
