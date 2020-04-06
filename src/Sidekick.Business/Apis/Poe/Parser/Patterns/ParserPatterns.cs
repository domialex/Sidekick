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
                { Models.Rarity.Normal, languageProvider.Language.RarityNormal.ToEndOfLineRegex() },
                { Models.Rarity.Magic, languageProvider.Language.RarityMagic.ToEndOfLineRegex() },
                { Models.Rarity.Rare, languageProvider.Language.RarityRare.ToEndOfLineRegex() },
                { Models.Rarity.Unique, languageProvider.Language.RarityUnique.ToEndOfLineRegex() },
                { Models.Rarity.Currency, languageProvider.Language.RarityCurrency.ToEndOfLineRegex() },
                { Models.Rarity.Gem, languageProvider.Language.RarityGem.ToEndOfLineRegex() },
                { Models.Rarity.DivinationCard, languageProvider.Language.RarityDivinationCard.ToEndOfLineRegex() }
            };

            ItemLevel = languageProvider.Language.DescriptionItemLevel.ToIntFromLineRegex();
            Unidentified = languageProvider.Language.DescriptionUnidentified.ToCompiledRegex(prefix: "[\\r\\n]");
            Corrupted = languageProvider.Language.DescriptionCorrupted.ToCompiledRegex(prefix: "[\\r\\n]");
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
            Armor = languageProvider.Language.DescriptionArmour.ToIntFromLineRegex();
            EnergyShield = languageProvider.Language.DescriptionEnergyShield.ToIntFromLineRegex();
            Evasion = languageProvider.Language.DescriptionEvasion.ToIntFromLineRegex();
            ChanceToBlock = languageProvider.Language.DescriptionChanceToBlock.ToIntFromLineRegex();
            Level = languageProvider.Language.DescriptionLevel.ToIntFromLineRegex();
            AttacksPerSecond = languageProvider.Language.DescriptionAttacksPerSecond.ToDecimalFromLineRegex();
            CriticalStrikeChance = languageProvider.Language.DescriptionCriticalStrikeChance.ToDecimalFromLineRegex();
            ElementalDamage = languageProvider.Language.DescriptionElementalDamage.ToRangeFromLineRegex();
            PhysicalDamage = languageProvider.Language.DescriptionPhysicalDamage.ToRangeFromLineRegex();

            Quality = languageProvider.Language.DescriptionQuality.ToIntFromLineRegex();

            MapTier = languageProvider.Language.DescriptionMapTier.ToIntFromLineRegex();
            ItemQuantity = languageProvider.Language.DescriptionItemQuantity.ToIntFromLineRegex();
            ItemRarity = languageProvider.Language.DescriptionItemRarity.ToIntFromLineRegex();
            MonsterPackSize = languageProvider.Language.DescriptionMonsterPackSize.ToIntFromLineRegex();
            Blighted = languageProvider.Language.PrefixBlighted.ToCompiledRegex("[\\ \\r\\n]", "[\\ \\r\\n]");
        }
        #endregion

        #region Sockets
        public Regex Socket { get; private set; }

        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            Socket = languageProvider.Language.DescriptionSockets.ToCompiledRegex(suffix: "[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
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
            Crusader = languageProvider.Language.InfluenceCrusader.ToStartOfLineRegex();
            Elder = languageProvider.Language.InfluenceElder.ToStartOfLineRegex();
            Hunter = languageProvider.Language.InfluenceHunter.ToStartOfLineRegex();
            Redeemer = languageProvider.Language.InfluenceRedeemer.ToStartOfLineRegex();
            Shaper = languageProvider.Language.InfluenceShaper.ToStartOfLineRegex();
            Warlord = languageProvider.Language.InfluenceWarlord.ToStartOfLineRegex();
        }
        #endregion
    }
}
