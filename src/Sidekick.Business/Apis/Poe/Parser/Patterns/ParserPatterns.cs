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
                { Models.Rarity.Normal, new Regex(Regex.Escape(languageProvider.Language.RarityNormal) + "$") },
                { Models.Rarity.Magic, new Regex(Regex.Escape(languageProvider.Language.RarityMagic) + "$") },
                { Models.Rarity.Rare, new Regex(Regex.Escape(languageProvider.Language.RarityRare) + "$") },
                { Models.Rarity.Unique, new Regex(Regex.Escape(languageProvider.Language.RarityUnique) + "$") },
                { Models.Rarity.Currency, new Regex(Regex.Escape(languageProvider.Language.RarityCurrency) + "$") },
                { Models.Rarity.Gem, new Regex(Regex.Escape(languageProvider.Language.RarityGem) + "$") },
                { Models.Rarity.DivinationCard, new Regex(Regex.Escape(languageProvider.Language.RarityDivinationCard) + "$") }
            };

            ItemLevel = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionItemLevel)}[^\\r\\n\\d]*(\\d+)");
            Unidentified = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionUnidentified)}");
            Corrupted = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionCorrupted)}");
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
            Armor = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionArmour)}[^\\r\\n\\d]*(\\d+)");
            EnergyShield = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionEnergyShield)}[^\\r\\n\\d]*(\\d+)");
            Evasion = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionEvasion)}[^\\r\\n\\d]*(\\d+)");
            ChanceToBlock = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionChanceToBlock)}[^\\r\\n\\d]*(\\d+)");
            Quality = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionQuality)}[^\\r\\n\\d]*(\\d+)");
            Level = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionLevel)}[^\\r\\n\\d]*(\\d+)");
            MapTier = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionMapTier)}[^\\r\\n\\d]*(\\d+)");
            ItemQuantity = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionItemQuantity)}[^\\r\\n\\d]*(\\d+)");
            ItemRarity = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionItemRarity)}[^\\r\\n\\d]*(\\d+)");
            MonsterPackSize = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionMonsterPackSize)}[^\\r\\n\\d]*(\\d+)");
            AttacksPerSecond = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionAttacksPerSecond)}[^\\r\\n\\d]*([\\d,\\.]+)");
            CriticalStrikeChance = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionCriticalStrikeChance)}[^\\r\\n\\d]*([\\d,\\.]+)");
            ElementalDamage = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionElementalDamage)}[^\\r\\n\\d]*(\\d+-\\d+)");
            PhysicalDamage = new Regex($"[\\r\\n]{Regex.Escape(languageProvider.Language.DescriptionPhysicalDamage)}[^\\r\\n\\d]*(\\d+-\\d+)");
            Blighted = new Regex($"[\\ \\r\\n]{Regex.Escape(languageProvider.Language.PrefixBlighted)}[\\ \\r\\n]");
        }
        #endregion

        #region Sockets
        public Regex Socket { get; private set; }

        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            Socket = new Regex($"{Regex.Escape(languageProvider.Language.DescriptionSockets)}[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
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
            Crusader = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceCrusader)}");
            Elder = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceElder)}");
            Hunter = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceHunter)}");
            Redeemer = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceRedeemer)}");
            Shaper = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceShaper)}");
            Warlord = new Regex($"[\\r\\n]+{Regex.Escape(languageProvider.Language.InfluenceWarlord)}");
        }
        #endregion
    }
}
