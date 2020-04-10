using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Languages;
using Sidekick.Core.Initialization;

namespace Sidekick.Business.Apis.Poe.Parser.Patterns
{
    public class ParserPatterns : IOnAfterInit, IParserPatterns
    {
        private readonly ILanguageProvider languageProvider;

        public ParserPatterns(ILanguageProvider languageProvider)
        {
            this.languageProvider = languageProvider;
        }

        public Task OnAfterInit()
        {
            InitHeader();
            InitProperties();
            InitSockets();
            InitInfluences();

            return Task.CompletedTask;
        }

        #region Header (Rarity, Name, Type)
        public Dictionary<Rarity, Regex> Rarity { get; private set; }
        public Regex ItemLevel { get; private set; }
        public Regex Unidentified { get; private set; }
        public Regex Corrupted { get; private set; }

        private void InitHeader()
        {
            Rarity = new Dictionary<Rarity, Regex>
            {
                { Models.Rarity.Normal, languageProvider.Language.RarityNormal.EndOfLineRegex() },
                { Models.Rarity.Magic, languageProvider.Language.RarityMagic.EndOfLineRegex() },
                { Models.Rarity.Rare, languageProvider.Language.RarityRare.EndOfLineRegex() },
                { Models.Rarity.Unique, languageProvider.Language.RarityUnique.EndOfLineRegex() },
                { Models.Rarity.Currency, languageProvider.Language.RarityCurrency.EndOfLineRegex() },
                { Models.Rarity.Gem, languageProvider.Language.RarityGem.EndOfLineRegex() },
                { Models.Rarity.DivinationCard, languageProvider.Language.RarityDivinationCard.EndOfLineRegex() }
            };

            ItemLevel = languageProvider.Language.DescriptionItemLevel.IntFromLineRegex();
            Unidentified = languageProvider.Language.DescriptionUnidentified.ToRegex(prefix: "[\\r\\n]");
            Corrupted = languageProvider.Language.DescriptionCorrupted.ToRegex(prefix: "[\\r\\n]");
        }
        #endregion

        #region Properties (Armour, Evasion, Energy Shield, Quality, Level)
        public Regex Armor { get; private set; }
        public Regex EnergyShield { get; private set; }
        public Regex Evasion { get; private set; }
        public Regex ChanceToBlock { get; private set; }
        public Regex Quality { get; private set; }
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

        private void InitProperties()
        {
            Armor = languageProvider.Language.DescriptionArmour.ToRegex(prefix: "(?:^|[\\r\\n])", suffix: "[^\\r\\n\\d]*(\\d+)");

            EnergyShield = languageProvider.Language.DescriptionEnergyShield.IntFromLineRegex();
            Evasion = languageProvider.Language.DescriptionEvasion.IntFromLineRegex();
            ChanceToBlock = languageProvider.Language.DescriptionChanceToBlock.IntFromLineRegex();
            Level = languageProvider.Language.DescriptionLevel.IntFromLineRegex();
            AttacksPerSecond = languageProvider.Language.DescriptionAttacksPerSecond.DecimalFromLineRegex();
            CriticalStrikeChance = languageProvider.Language.DescriptionCriticalStrikeChance.DecimalFromLineRegex();
            ElementalDamage = languageProvider.Language.DescriptionElementalDamage.RangeFromLineRegex();
            PhysicalDamage = languageProvider.Language.DescriptionPhysicalDamage.RangeFromLineRegex();

            Quality = languageProvider.Language.DescriptionQuality.IntFromLineRegex();

            MapTier = languageProvider.Language.DescriptionMapTier.IntFromLineRegex();
            ItemQuantity = languageProvider.Language.DescriptionItemQuantity.IntFromLineRegex();
            ItemRarity = languageProvider.Language.DescriptionItemRarity.IntFromLineRegex();
            MonsterPackSize = languageProvider.Language.DescriptionMonsterPackSize.IntFromLineRegex();
            Blighted = languageProvider.Language.PrefixBlighted.ToRegex("[\\ \\r\\n]", "[\\ \\r\\n]");
        }
        #endregion

        #region Sockets
        public Regex Socket { get; private set; }

        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            Socket = languageProvider.Language.DescriptionSockets.ToRegex(suffix: "[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
        }
        #endregion

        #region Influences
        public Regex Crusader { get; private set; }
        public Regex Elder { get; private set; }
        public Regex Hunter { get; private set; }
        public Regex Redeemer { get; private set; }
        public Regex Shaper { get; private set; }
        public Regex Warlord { get; private set; }

        private void InitInfluences()
        {
            Crusader = languageProvider.Language.InfluenceCrusader.StartOfLineRegex();
            Elder = languageProvider.Language.InfluenceElder.StartOfLineRegex();
            Hunter = languageProvider.Language.InfluenceHunter.StartOfLineRegex();
            Redeemer = languageProvider.Language.InfluenceRedeemer.StartOfLineRegex();
            Shaper = languageProvider.Language.InfluenceShaper.StartOfLineRegex();
            Warlord = languageProvider.Language.InfluenceWarlord.StartOfLineRegex();
        }
        #endregion

        #region Helpers
        public int GetInt(Regex regex, string input)
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

        public double GetDouble(Regex regex, string input)
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

        public double GetDps(Regex regex, string input, double attacksPerSecond)
        {
            if (regex != null)
            {
                var match = regex.Match(input);

                if (match.Success)
                {
                    var split = match.Groups[1].Value.Split('-');

                    if (int.TryParse(split[0], out var minValue) && int.TryParse(split[1], out var maxValue))
                    {
                        return ((minValue + maxValue) / 2) * attacksPerSecond;
                    }
                }
            }

            return 0;
        }
        #endregion
    }
}
