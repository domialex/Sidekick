using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Modifiers;
using Sidekick.Domain.Game.Modifiers.Models;
using Sidekick.Domain.Game.Trade;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Domain.Settings;
using Sidekick.Infrastructure.PoeApi.Trade.Search.Filters;
using Sidekick.Infrastructure.PoeApi.Trade.Search.Requests;
using Sidekick.Infrastructure.PoeApi.Trade.Search.Results;

namespace Sidekick.Infrastructure.PoeApi.Trade.Search
{
    public class TradeSearchService : ITradeSearchService
    {
        private readonly ILogger logger;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ISidekickSettings settings;
        private readonly IPoeTradeClient poeTradeClient;
        private readonly IItemStaticDataProvider itemStaticDataProvider;
        private readonly IModifierProvider modifierProvider;

        public TradeSearchService(ILogger<TradeSearchService> logger,
            IGameLanguageProvider gameLanguageProvider,
            ISidekickSettings settings,
            IPoeTradeClient poeTradeClient,
            IItemStaticDataProvider itemStaticDataProvider,
            IModifierProvider modifierProvider)
        {
            this.logger = logger;
            this.gameLanguageProvider = gameLanguageProvider;
            this.settings = settings;
            this.poeTradeClient = poeTradeClient;
            this.itemStaticDataProvider = itemStaticDataProvider;
            this.modifierProvider = modifierProvider;
        }

        public async Task<TradeSearchResult<string>> SearchBulk(Item item)
        {
            try
            {
                logger.LogInformation("Querying Exchange API.");

                var uri = $"{gameLanguageProvider.Language.PoeTradeApiBaseUrl}exchange/{settings.LeagueId}";
                var json = JsonSerializer.Serialize(new BulkQueryRequest(item, itemStaticDataProvider), poeTradeClient.Options);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await poeTradeClient.HttpClient.PostAsync(uri, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<TradeSearchResult<string>>(content, poeTradeClient.Options);
                }
                else
                {
                    var responseMessage = await response?.Content?.ReadAsStringAsync();
                    logger.LogError("Querying failed: {responseCode} {responseMessage}", response.StatusCode, responseMessage);
                    logger.LogError("Uri: {uri}", uri);
                    logger.LogError("Query: {query}", json);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception thrown while querying trade api.");
            }

            return null;
        }

        public async Task<TradeSearchResult<string>> Search(Item item, List<PropertyFilter> propertyFilters = null, List<ModifierFilter> modifierFilters = null)
        {
            try
            {
                logger.LogInformation("Querying Trade API.");

                var request = new QueryRequest();
                SetFilters(request.Query.Filters, propertyFilters);
                SetStats(request.Query.Stats, modifierFilters);

                // Auto Search 5+ Links
                var highestCount = item.Sockets
                    .GroupBy(x => x.Group)
                    .Select(x => x.Count())
                    .OrderByDescending(x => x)
                    .FirstOrDefault();
                if (highestCount >= 5)
                {
                    request.Query.Filters.SocketFilters.Filters.Links = new SocketFilterOption()
                    {
                        Min = highestCount,
                    };
                }

                if (item.Rarity == Rarity.Unique)
                {
                    request.Query.Name = item.Name;
                    request.Query.Type = item.Type;
                    request.Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption("Unique");
                }
                else if (item.Rarity == Rarity.Prophecy)
                {
                    request.Query.Name = item.Name;
                }
                else
                {
                    if (string.IsNullOrEmpty(request.Query.Filters.TypeFilters.Filters.Category?.Option))
                    {
                        request.Query.Type = item.Type;
                    }
                    request.Query.Filters.TypeFilters.Filters.Rarity = new SearchFilterOption("nonunique");
                }

                if (item.Properties.AlternateQuality)
                {
                    request.Query.Term = item.NameLine;
                }

                if (item.Properties.MapTier > 0)
                {
                    request.Query.Filters.MapFilters.Filters.MapTier = new SearchFilterValue(item.Properties.MapTier, item.Properties.MapTier);
                }

                var uri = new Uri($"{gameLanguageProvider.Language.PoeTradeApiBaseUrl}search/{settings.LeagueId}");
                var json = JsonSerializer.Serialize(request, poeTradeClient.Options);
                var body = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await poeTradeClient.HttpClient.PostAsync(uri, body);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    return await JsonSerializer.DeserializeAsync<TradeSearchResult<string>>(content, poeTradeClient.Options);
                }
                else
                {
                    var responseMessage = await response?.Content?.ReadAsStringAsync();
                    logger.LogError("Querying failed: {responseCode} {responseMessage}", response.StatusCode, responseMessage);
                    logger.LogError("Uri: {uri}", uri);
                    logger.LogError("Query: {query}", json);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Exception thrown while querying trade api.");
            }

            return null;
        }

        private void SetFilters(SearchFilters filters, List<PropertyFilter> propertyFilters)
        {
            if (propertyFilters == null)
            {
                return;
            }

            foreach (var propertyFilter in propertyFilters)
            {
                if (!propertyFilter.Enabled
                    && propertyFilter.Type != PropertyFilterType.Misc_Corrupted)
                {
                    continue;
                }

                switch (propertyFilter.Type)
                {
                    // Armour
                    case PropertyFilterType.Armour_Armour:
                        filters.ArmourFilters.Filters.Armor = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Armour_Block:
                        filters.ArmourFilters.Filters.Block = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Armour_EnergyShield:
                        filters.ArmourFilters.Filters.EnergyShield = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Armour_Evasion:
                        filters.ArmourFilters.Filters.Evasion = new SearchFilterValue(propertyFilter);
                        break;

                    // Category
                    case PropertyFilterType.Category:
                        filters.TypeFilters.Filters.Category = new SearchFilterOption(propertyFilter);
                        break;

                    // Influence
                    case PropertyFilterType.Influence_Crusader:
                        filters.MiscFilters.Filters.CrusaderItem = new SearchFilterOption(propertyFilter);
                        break;
                    case PropertyFilterType.Influence_Elder:
                        filters.MiscFilters.Filters.ElderItem = new SearchFilterOption(propertyFilter);
                        break;
                    case PropertyFilterType.Influence_Hunter:
                        filters.MiscFilters.Filters.HunterItem = new SearchFilterOption(propertyFilter);
                        break;
                    case PropertyFilterType.Influence_Redeemer:
                        filters.MiscFilters.Filters.RedeemerItem = new SearchFilterOption(propertyFilter);
                        break;
                    case PropertyFilterType.Influence_Shaper:
                        filters.MiscFilters.Filters.ShaperItem = new SearchFilterOption(propertyFilter);
                        break;
                    case PropertyFilterType.Influence_Warlord:
                        filters.MiscFilters.Filters.WarlordItem = new SearchFilterOption(propertyFilter);
                        break;

                    // Map
                    case PropertyFilterType.Map_ItemQuantity:
                        filters.MapFilters.Filters.ItemQuantity = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Map_ItemRarity:
                        filters.MapFilters.Filters.ItemRarity = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Map_MonsterPackSize:
                        filters.MapFilters.Filters.MonsterPackSize = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Map_Blighted:
                        filters.MapFilters.Filters.Blighted = new SearchFilterOption(propertyFilter);
                        break;
                    case PropertyFilterType.Map_Tier:
                        filters.MapFilters.Filters.MapTier = new SearchFilterValue(propertyFilter);
                        break;

                    // Misc
                    case PropertyFilterType.Misc_Quality:
                        filters.MiscFilters.Filters.Quality = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Misc_GemLevel:
                        filters.MiscFilters.Filters.GemLevel = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Misc_ItemLevel:
                        filters.MiscFilters.Filters.ItemLevel = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Misc_Corrupted:
                        filters.MiscFilters.Filters.Corrupted = new SearchFilterOption(propertyFilter);
                        break;

                    // Weapon
                    case PropertyFilterType.Weapon_PhysicalDps:
                        filters.WeaponFilters.Filters.PhysicalDps = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Weapon_ElementalDps:
                        filters.WeaponFilters.Filters.ElementalDps = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Weapon_Dps:
                        filters.WeaponFilters.Filters.DamagePerSecond = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Weapon_AttacksPerSecond:
                        filters.WeaponFilters.Filters.AttacksPerSecond = new SearchFilterValue(propertyFilter);
                        break;
                    case PropertyFilterType.Weapon_CriticalStrikeChance:
                        filters.WeaponFilters.Filters.CriticalStrikeChance = new SearchFilterValue(propertyFilter);
                        break;
                }
            }
        }

        private void SetStats(List<StatFilterGroup> stats, List<ModifierFilter> modifierFilters)
        {
            if (modifierFilters == null)
            {
                return;
            }

            stats.Add(new StatFilterGroup()
            {
                Type = StatType.And,
                Filters = modifierFilters.ConvertAll(x => new StatFilter()
                {
                    Disabled = false,
                    Id = x.Id,
                })
            });
        }

        public async Task<List<TradeItem>> GetResults(string queryId, List<string> ids, List<ModifierFilter> modifierFilters = null)
        {
            try
            {
                logger.LogInformation($"Fetching Trade API Listings from Query {queryId}.");

                var pseudo = string.Empty;
                if (modifierFilters != null)
                {
                    pseudo = string.Join("", modifierFilters.Where(x => x.Id.StartsWith("pseudo.")).Select(x => $"&pseudos[]={x.Id}"));
                }

                var response = await poeTradeClient.HttpClient.GetAsync(gameLanguageProvider.Language.PoeTradeApiBaseUrl + "fetch/" + string.Join(",", ids) + "?query=" + queryId + pseudo);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStreamAsync();
                    var result = await JsonSerializer.DeserializeAsync<FetchResult<Result>>(content, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    });

                    return result.Result.ConvertAll(x => GetItem(x));
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception thrown when fetching trade API listings from Query {queryId}.");
            }

            return null;
        }

        private TradeItem GetItem(Result result)
        {
            var item = new TradeItem()
            {
                Id = result.Id,

                Price = new TradePrice()
                {
                    AccountCharacter = result.Listing.Account.LastCharacterName,
                    AccountName = result.Listing.Account.Name,
                    Amount = result.Listing.Price.Amount,
                    Currency = result.Listing.Price.Currency,
                    Date = result.Listing.Indexed,
                    Whisper = result.Listing.Whisper,
                },

                Corrupted = result.Item.Corrupted,
                Identified = result.Item.Identified,
                Influences = result.Item.Influences,
                ItemLevel = result.Item.ItemLevel,
                Name = result.Item.Name,
                NameLine = result.Item.Name,
                Rarity = result.Item.Rarity,
                Type = result.Item.TypeLine,
                TypeLine = result.Item.TypeLine,

                Icon = result.Item.Icon,
                Note = result.Item.Note,

                PropertyTexts = ParseLineContents(result.Item.Properties),
                Requirements = ParseLineContents(result.Item.Requirements),
                Sockets = ParseSockets(result.Item.Sockets),

                Properties = new Properties()
                {
                    Armor = result.Item.Extended.ArmourAtMax,
                    EnergyShield = result.Item.Extended.EnergyShieldAtMax,
                    Evasion = result.Item.Extended.EvasionAtMax,
                    DamagePerSecond = result.Item.Extended.DamagePerSecond,
                    ElementalDps = result.Item.Extended.ElementalDps,
                    PhysicalDps = result.Item.Extended.PhysicalDps,
                },
            };

            ParseMods(modifierProvider,
                item.Modifiers.Crafted,
                result.Item.CraftedMods,
                result.Item.Extended.Mods?.Crafted,
                ParseHash(result.Item.Extended.Hashes?.Crafted));

            ParseMods(modifierProvider,
                item.Modifiers.Enchant,
                result.Item.EnchantMods,
                result.Item.Extended.Mods?.Enchant,
                ParseHash(result.Item.Extended.Hashes?.Enchant));

            ParseMods(modifierProvider,
                item.Modifiers.Explicit,
                result.Item.ExplicitMods,
                result.Item.Extended.Mods?.Explicit,
                ParseHash(result.Item.Extended.Hashes?.Explicit));

            ParseMods(modifierProvider,
                item.Modifiers.Implicit,
                result.Item.ImplicitMods,
                result.Item.Extended.Mods?.Implicit,
                ParseHash(result.Item.Extended.Hashes?.Implicit));

            ParseMods(modifierProvider,
                item.Modifiers.Pseudo,
                result.Item.PseudoMods,
                result.Item.Extended.Mods?.Pseudo,
                ParseHash(result.Item.Extended.Hashes?.Pseudo));

            ParseMods(modifierProvider,
                item.Modifiers.Fractured,
                result.Item.FracturedMods,
                result.Item.Extended.Mods?.Fractured,
                ParseHash(result.Item.Extended.Hashes?.Fractured));

            return item;
        }

        private List<LineContentValue> ParseHash(List<List<JsonElement>> values)
        {
            var result = new List<LineContentValue>();
            if (values != null)
            {
                foreach (var value in values)
                {
                    if (value.Count != 2)
                    {
                        continue;
                    }

                    result.Add(new LineContentValue()
                    {
                        Value = value[0].GetString(),
                        Type = value[1].ValueKind == JsonValueKind.Array ? (LineContentType)value[1][0].GetInt32() : LineContentType.Simple
                    });
                }
            }
            return result;
        }

        private List<LineContent> ParseLineContents(List<ResultLineContent> lines)
        {
            return lines
                .OrderBy(x => x.Order)
                .Select(line =>
                {
                    var values = new List<LineContentValue>();
                    foreach (var value in line.Values)
                    {
                        if (line.Values.Count != 2)
                        {
                            continue;
                        }

                        values.Add(new LineContentValue()
                        {
                            Value = value[0].GetString(),
                            Type = (LineContentType)value[1].GetInt32()
                        });
                    }

                    var text = line.Name;

                    if (values.Count > 0)
                    {
                        switch (line.DisplayMode)
                        {
                            case 0:
                                text = $"{line.Name}: {string.Join(", ", values.Select(x => x.Value))}";
                                break;
                            case 1:
                                text = $"{values[0].Value} {line.Name}";
                                break;
                            case 2:
                                text = $"{values[0].Value}";
                                break;
                            case 3:
                                var format = Regex.Replace(line.Name, "%(\\d)", "{$1}");
                                text = string.Format(format, values.Select(x => x.Value).ToArray());
                                break;
                            default:
                                text = $"{line.Name} {string.Join(", ", values.Select(x => x.Value))}";
                                break;
                        }
                    }

                    return new LineContent()
                    {
                        Text = text,
                        Values = values,
                    };
                })
                .ToList();
        }

        private void ParseMods(IModifierProvider modifierProvider, List<Modifier> modifiers, List<string> texts, List<Mod> mods, List<LineContentValue> hashes)
        {
            if (modifiers == null || mods == null || hashes == null)
            {
                return;
            }

            for (var index = 0; index < hashes.Count; index++)
            {
                var id = hashes[index].Value;
                var definition = modifierProvider.GetById(id);

                var text = texts.FirstOrDefault(x => modifierProvider.IsMatch(id, x));
                var mod = mods.FirstOrDefault(x => x.Magnitudes != null && x.Magnitudes.Any(y => y.Hash == definition.Id));

                modifiers.Add(new Modifier()
                {
                    Id = definition.Id,
                    Text = text ?? definition.Text,
                    Tier = mod?.Tier,
                    TierName = mod?.Name,
                });
            }
        }

        private List<Socket> ParseSockets(List<ResultSocket> sockets)
        {
            return sockets.ConvertAll(x => new Socket()
            {
                Group = x.Group,
                Colour = x.ColourString switch
                {
                    "B" => SocketColour.Blue,
                    "G" => SocketColour.Green,
                    "R" => SocketColour.Red,
                    "W" => SocketColour.White,
                    "A" => SocketColour.Abyss,
                    _ => throw new Exception("Invalid socket"),
                }
            });
        }
    }
}
