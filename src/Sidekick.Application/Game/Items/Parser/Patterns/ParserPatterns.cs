using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Sidekick.Application.Game.Items.Parser.Extensions;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;

namespace Sidekick.Application.Game.Items.Parser.Patterns
{
    public class ParserPatterns : IParserPatterns
    {
        private readonly IGameLanguageProvider gameLanguageProvider;

        public ParserPatterns(IGameLanguageProvider gameLanguageProvider)
        {
            this.gameLanguageProvider = gameLanguageProvider;
        }

        public void Initialize()
        {
            InitHeader();
            InitProperties();
            InitSockets();
            InitInfluences();
        }

        #region Header (Rarity, Name, Type)
        private void InitHeader()
        {
            Rarity = new Dictionary<Rarity, Regex>
            {
                { Domain.Game.Items.Models.Rarity.Normal, gameLanguageProvider.Language.RarityNormal.EndOfLineRegex() },
                { Domain.Game.Items.Models.Rarity.Magic, gameLanguageProvider.Language.RarityMagic.EndOfLineRegex() },
                { Domain.Game.Items.Models.Rarity.Rare, gameLanguageProvider.Language.RarityRare.EndOfLineRegex() },
                { Domain.Game.Items.Models.Rarity.Unique, gameLanguageProvider.Language.RarityUnique.EndOfLineRegex() },
                { Domain.Game.Items.Models.Rarity.Currency, gameLanguageProvider.Language.RarityCurrency.EndOfLineRegex() },
                { Domain.Game.Items.Models.Rarity.Gem, gameLanguageProvider.Language.RarityGem.EndOfLineRegex() },
                { Domain.Game.Items.Models.Rarity.DivinationCard, gameLanguageProvider.Language.RarityDivinationCard.EndOfLineRegex() }
            };

            ItemLevel = gameLanguageProvider.Language.DescriptionItemLevel.IntFromLineRegex();
            Unidentified = gameLanguageProvider.Language.DescriptionUnidentified.ToRegex(prefix: "[\\r\\n]");
            Corrupted = gameLanguageProvider.Language.DescriptionCorrupted.ToRegex(prefix: "[\\r\\n]");
        }
        public Dictionary<Rarity, Regex> Rarity { get; private set; }
        public Regex ItemLevel { get; private set; }
        public Regex Unidentified { get; private set; }
        public Regex Corrupted { get; private set; }
        #endregion Header (Rarity, Name, Type)

        #region Properties (Armour, Evasion, Energy Shield, Quality, Level)
        private void InitProperties()
        {
            Armor = gameLanguageProvider.Language.DescriptionArmour.ToRegex(prefix: "(?:^|[\\r\\n])", suffix: "[^\\r\\n\\d]*(\\d+)");

            EnergyShield = gameLanguageProvider.Language.DescriptionEnergyShield.IntFromLineRegex();
            Evasion = gameLanguageProvider.Language.DescriptionEvasion.IntFromLineRegex();
            ChanceToBlock = gameLanguageProvider.Language.DescriptionChanceToBlock.IntFromLineRegex();
            Level = gameLanguageProvider.Language.DescriptionLevel.IntFromLineRegex();
            AttacksPerSecond = gameLanguageProvider.Language.DescriptionAttacksPerSecond.DecimalFromLineRegex();
            CriticalStrikeChance = gameLanguageProvider.Language.DescriptionCriticalStrikeChance.DecimalFromLineRegex();
            ElementalDamage = gameLanguageProvider.Language.DescriptionElementalDamage.LineRegex();
            PhysicalDamage = gameLanguageProvider.Language.DescriptionPhysicalDamage.LineRegex();

            Quality = gameLanguageProvider.Language.DescriptionQuality.IntFromLineRegex();
            AlternateQuality = gameLanguageProvider.Language.DescriptionAlternateQuality.ToRegex();

            MapTier = gameLanguageProvider.Language.DescriptionMapTier.IntFromLineRegex();
            ItemQuantity = gameLanguageProvider.Language.DescriptionItemQuantity.IntFromLineRegex();
            ItemRarity = gameLanguageProvider.Language.DescriptionItemRarity.IntFromLineRegex();
            MonsterPackSize = gameLanguageProvider.Language.DescriptionMonsterPackSize.IntFromLineRegex();
            Blighted = gameLanguageProvider.Language.PrefixBlighted.ToRegex("[\\ \\r\\n]", "[\\ \\r\\n]");
        }

        public Regex Armor { get; private set; }
        public Regex EnergyShield { get; private set; }
        public Regex Evasion { get; private set; }
        public Regex ChanceToBlock { get; private set; }
        public Regex Quality { get; private set; }
        public Regex AlternateQuality { get; private set; }
        public Regex Level { get; private set; }
        public Regex MapTier { get; private set; }
        public Regex ItemQuantity { get; private set; }
        public Regex ItemRarity { get; private set; }
        public Regex MonsterPackSize { get; private set; }
        public Regex AttacksPerSecond { get; private set; }
        public Regex CriticalStrikeChance { get; private set; }
        public Regex ElementalDamage { get; private set; }
        public Regex PhysicalDamage { get; private set; }
        public Regex Blighted { get; private set; }
        #endregion Properties (Armour, Evasion, Energy Shield, Quality, Level)

        #region Sockets
        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            Socket = gameLanguageProvider.Language.DescriptionSockets.ToRegex(suffix: "[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
        }

        public Regex Socket { get; private set; }
        #endregion Sockets

        #region Influences
        private void InitInfluences()
        {
            Crusader = gameLanguageProvider.Language.InfluenceCrusader.ToRegex(prefix: "[\\r\\n]+");
            Elder = gameLanguageProvider.Language.InfluenceElder.ToRegex(prefix: "[\\r\\n]+");
            Hunter = gameLanguageProvider.Language.InfluenceHunter.ToRegex(prefix: "[\\r\\n]+");
            Redeemer = gameLanguageProvider.Language.InfluenceRedeemer.ToRegex(prefix: "[\\r\\n]+");
            Shaper = gameLanguageProvider.Language.InfluenceShaper.ToRegex(prefix: "[\\r\\n]+");
            Warlord = gameLanguageProvider.Language.InfluenceWarlord.ToRegex(prefix: "[\\r\\n]+");
        }

        public Regex Crusader { get; private set; }
        public Regex Elder { get; private set; }
        public Regex Hunter { get; private set; }
        public Regex Redeemer { get; private set; }
        public Regex Shaper { get; private set; }
        public Regex Warlord { get; private set; }
        #endregion Influences

        #region Helpers
        public int GetInt(Regex regex, string input)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success && int.TryParse(match.Groups[1].Value, out var result))
                {
                    return result;
                }
            }

            return 0;
        }

        public double GetDouble(Regex regex, string input)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success && double.TryParse(match.Groups[1].Value.Replace(",", "."), NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                {
                    return result;
                }
            }

            return 0;
        }

        public double GetDps(Regex regex, string input, double attacksPerSecond)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    var matches = new Regex("(\\d+-\\d+)").Matches(match.Value);
                    var dps = matches
                        .Select(x => x.Value.Split("-"))
                        .Sum(split =>
                        {
                            if (double.TryParse(split[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var minValue)
                             && double.TryParse(split[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var maxValue))
                            {
                                return (minValue + maxValue) / 2d;
                            }

                            return 0d;
                        });

                    return Math.Round(dps * attacksPerSecond, 2);
                }
            }

            return 0d;
        }
        #endregion Helpers
    }
}
