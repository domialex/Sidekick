using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Languages;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public class ParserService : IOnAfterInit, IParserService
    {
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IStatDataService statsDataService;
        private readonly IItemDataService itemDataService;

        public ParserService(
            ILogger logger,
            ILanguageProvider languageProvider,
            IStatDataService statsDataService,
            IItemDataService itemDataService)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.statsDataService = statsDataService;
            this.itemDataService = itemDataService;
        }

        private Regex NewlinePattern { get; set; }
        private Regex SeparatorPattern { get; set; }

        public Task OnAfterInit()
        {
            NewlinePattern = new Regex("[\\r\\n]+");
            SeparatorPattern = new Regex("--------");

            InitHeader();
            InitProperties();
            InitSockets();
            InitInfluences();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Tries to parse an item based on the text that Path of Exile gives on a Ctrl+C action.
        /// There is no recurring logic here so every case has to be handled manually.
        /// </summary>
        public async Task<ParsedItem> ParseItem(string itemText)
        {
            await languageProvider.FindAndSetLanguage(itemText);

            try
            {
                itemText = new ItemNameTokenizer().CleanString(itemText);
                var item = new ParsedItem();

                ParseHeader(ref item, ref itemText);
                ParseProperties(ref item, ref itemText);
                ParseSockets(ref item, ref itemText);
                ParseInfluences(ref item, ref itemText);

                return item;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not parse item.");
                return null;
            }
        }

        #region Header (Rarity, Name, Type)
        private Dictionary<Rarity, Regex> RarityPatterns { get; set; }
        private Regex UnidentifiedPattern { get; set; }
        private Regex CorruptedPattern { get; set; }

        private void InitHeader()
        {
            RarityPatterns = new Dictionary<Rarity, Regex>
            {
                { Rarity.Normal, new Regex(Regex.Escape(languageProvider.Language.RarityNormal)) },
                { Rarity.Magic, new Regex(Regex.Escape(languageProvider.Language.RarityMagic)) },
                { Rarity.Rare, new Regex(Regex.Escape(languageProvider.Language.RarityRare)) },
                { Rarity.Unique, new Regex(Regex.Escape(languageProvider.Language.RarityUnique)) },
                { Rarity.Currency, new Regex(Regex.Escape(languageProvider.Language.RarityCurrency)) },
                { Rarity.Gem, new Regex(Regex.Escape(languageProvider.Language.RarityGem)) },
                { Rarity.DivinationCard, new Regex(Regex.Escape(languageProvider.Language.RarityDivinationCard)) }
            };

            UnidentifiedPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionUnidentified)}");
            CorruptedPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionCorrupted)}");
        }

        private void ParseHeader(ref ParsedItem item, ref string input)
        {
            var lines = NewlinePattern.Split(input);
            var blocks = SeparatorPattern.Split(input);

            item.Rarity = GetRarity(lines[0]);

            var dataItem = itemDataService.GetItem(blocks[0]);

            item.Name = dataItem.Name;
            item.TypeLine = dataItem.Type;

            if (string.IsNullOrWhiteSpace(item.Name))
            {
                item.Name = lines[1];
            }

            item.Identified = !UnidentifiedPattern.IsMatch(input);
            item.Corrupted = CorruptedPattern.IsMatch(input);
        }

        private Rarity GetRarity(string rarityString)
        {
            foreach (var pattern in RarityPatterns)
            {
                if (pattern.Value.IsMatch(rarityString))
                {
                    return pattern.Key;
                }
            }
            throw new Exception("Can't parse rarity.");
        }
        #endregion

        #region Properties (Armour, Evasion, Energy Shield, Quality, Level)
        private Regex ArmorPattern { get; set; }
        private Regex EnergyShieldPattern { get; set; }
        private Regex EvasionPattern { get; set; }
        private Regex QualityPattern { get; set; }
        private Regex LevelPattern { get; set; }
        private Regex MapTierPattern { get; set; }
        private Regex ItemQuantityPattern { get; set; }
        private Regex ItemRarityPattern { get; set; }
        private Regex MonsterPackSizePattern { get; set; }
        private Regex AttacksPerSecondPattern { get; set; }
        private Regex CriticalStrikeChancePattern { get; set; }
        private Regex ElementalDamagePattern { get; set; }
        private Regex PhysicalDamagePattern { get; set; }
        private Regex BlightedPattern { get; set; }

        private void InitProperties()
        {
            ArmorPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionArmour)}[^\\r\\n\\d]*(\\d+)");
            EnergyShieldPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionEnergyShield)}[^\\r\\n\\d]*(\\d+)");
            EvasionPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionEvasion)}[^\\r\\n\\d]*(\\d+)");
            QualityPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionQuality)}[^\\r\\n\\d]*(\\d+)");
            LevelPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionLevel)}[^\\r\\n\\d]*(\\d+)");
            MapTierPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionMapTier)}[^\\r\\n\\d]*(\\d+)");
            ItemQuantityPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionItemQuantity)}[^\\r\\n\\d]*(\\d+)");
            ItemRarityPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionItemRarity)}[^\\r\\n\\d]*(\\d+)");
            MonsterPackSizePattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionMonsterPackSize)}[^\\r\\n\\d]*(\\d+)");
            AttacksPerSecondPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionAttacksPerSecond)}[^\\r\\n\\d]*([\\d,\\.]+)");
            CriticalStrikeChancePattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionCriticalStrikeChance)}[^\\r\\n\\d]*([\\d,\\.]+)");
            ElementalDamagePattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionElementalDamage)}([^\\r\\n]*)");
            PhysicalDamagePattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionPhysicalDamage)}([^\\r\\n]*)");
            BlightedPattern = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.PrefixBlighted)}");
        }

        private void ParseProperties(ref ParsedItem item, ref string input)
        {
            var blocks = SeparatorPattern.Split(input);

            item.Armor = GetInt(ArmorPattern, blocks[1]);
            item.EnergyShield = GetInt(EnergyShieldPattern, blocks[1]);
            item.Evasion = GetInt(EvasionPattern, blocks[1]);
            item.Quality = GetInt(QualityPattern, blocks[1]);
            item.Level = GetInt(LevelPattern, blocks[1]);
            item.MapTier = GetInt(MapTierPattern, blocks[1]);
            item.ItemQuantity = GetInt(ItemQuantityPattern, blocks[1]);
            item.ItemRarity = GetInt(ItemRarityPattern, blocks[1]);
            item.MonsterPackSize = GetInt(MonsterPackSizePattern, blocks[1]);
            item.AttacksPerSecond = GetDouble(AttacksPerSecondPattern, blocks[1]);
            item.CriticalStrikeChance = GetDouble(CriticalStrikeChancePattern, blocks[1]);
            item.ElementalDamage = GetString(ElementalDamagePattern, blocks[1]);
            item.PhysicalDamage = GetString(PhysicalDamagePattern, blocks[1]);
            item.Blighted = BlightedPattern.IsMatch(blocks[0]);
        }
        #endregion

        #region Sockets
        private Regex SocketPattern { get; set; }

        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            SocketPattern = new Regex($"{Regex.Escape(languageProvider.Language.DescriptionSockets)}[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
        }

        private void ParseSockets(ref ParsedItem item, ref string input)
        {
            var result = SocketPattern.Match(input);
            if (result.Success)
            {
                var groups = result.Groups
                    .Where(x => !string.IsNullOrEmpty(x.Value))
                    .Select(x => x.Value)
                    .ToList();
                for (var index = 1; index < groups.Count; index++)
                {
                    var groupValue = groups[index].Replace("-", "").Trim();
                    while (groupValue.Length > 0)
                    {
                        switch (groupValue[0])
                        {
                            case 'B': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Blue }); break;
                            case 'G': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Green }); break;
                            case 'R': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Red }); break;
                            case 'W': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.White }); break;
                            case 'A': item.Sockets.Add(new Socket() { Group = index - 1, Color = SocketColor.Abyss }); break;
                        }
                        groupValue = groupValue.Substring(1);
                    }
                }
            }
        }
        #endregion

        #region Influences
        private Regex CrusaderPattern { get; set; }
        private Regex ElderPattern { get; set; }
        private Regex HunterPattern { get; set; }
        private Regex RedeemerPattern { get; set; }
        private Regex ShaperPattern { get; set; }
        private Regex WarlordPattern { get; set; }

        private void InitInfluences()
        {
            CrusaderPattern = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceCrusader)}");
            ElderPattern = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceElder)}");
            HunterPattern = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceHunter)}");
            RedeemerPattern = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceRedeemer)}");
            ShaperPattern = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceShaper)}");
            WarlordPattern = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceWarlord)}");
        }

        private void ParseInfluences(ref ParsedItem item, ref string input)
        {
            item.Influences.Crusader = CrusaderPattern.IsMatch(input);
            item.Influences.Elder = ElderPattern.IsMatch(input);
            item.Influences.Hunter = HunterPattern.IsMatch(input);
            item.Influences.Redeemer = RedeemerPattern.IsMatch(input);
            item.Influences.Shaper = ShaperPattern.IsMatch(input);
            item.Influences.Warlord = WarlordPattern.IsMatch(input);
        }
        #endregion

        #region Helpers
        private int GetInt(Regex regex, string input)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    if (int.TryParse(match.Groups[1].Value, out var result))
                    {
                        return result;
                    }
                }
            }

            return 0;
        }

        private double GetDouble(Regex regex, string input)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    if (double.TryParse(match.Groups[1].Value.Replace(",", "."), out var result))
                    {
                        return result;
                    }
                }
            }

            return 0;
        }

        private string GetString(Regex regex, string input)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return string.Empty;
        }
        #endregion
    }
}
