using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Languages;

namespace Sidekick.Business.Apis.Poe.Parser.Patterns
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly ILanguageProvider languageProvider;
        private readonly IParserPatterns parserPatterns;

        public DataInitializationStartedHandler(
            ILanguageProvider languageProvider,
            IParserPatterns parserPatterns)
        {
            this.languageProvider = languageProvider;
            this.parserPatterns = parserPatterns;
        }

        public Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            InitHeader();
            InitProperties();
            InitSockets();
            InitInfluences();

            return Task.CompletedTask;
        }

        #region Header (Rarity, Name, Type)

        private void InitHeader()
        {
            parserPatterns.Rarity = new Dictionary<Rarity, Regex>
            {
                { Models.Rarity.Normal, languageProvider.Language.RarityNormal.EndOfLineRegex() },
                { Models.Rarity.Magic, languageProvider.Language.RarityMagic.EndOfLineRegex() },
                { Models.Rarity.Rare, languageProvider.Language.RarityRare.EndOfLineRegex() },
                { Models.Rarity.Unique, languageProvider.Language.RarityUnique.EndOfLineRegex() },
                { Models.Rarity.Currency, languageProvider.Language.RarityCurrency.EndOfLineRegex() },
                { Models.Rarity.Gem, languageProvider.Language.RarityGem.EndOfLineRegex() },
                { Models.Rarity.DivinationCard, languageProvider.Language.RarityDivinationCard.EndOfLineRegex() }
            };

            parserPatterns.ItemLevel = languageProvider.Language.DescriptionItemLevel.IntFromLineRegex();
            parserPatterns.Unidentified = languageProvider.Language.DescriptionUnidentified.ToRegex(prefix: "[\\r\\n]");
            parserPatterns.Corrupted = languageProvider.Language.DescriptionCorrupted.ToRegex(prefix: "[\\r\\n]");
        }

        #endregion Header (Rarity, Name, Type)

        #region Properties (Armour, Evasion, Energy Shield, Quality, Level)

        private void InitProperties()
        {
            parserPatterns.Armor = languageProvider.Language.DescriptionArmour.ToRegex(prefix: "(?:^|[\\r\\n])", suffix: "[^\\r\\n\\d]*(\\d+)");

            parserPatterns.EnergyShield = languageProvider.Language.DescriptionEnergyShield.IntFromLineRegex();
            parserPatterns.Evasion = languageProvider.Language.DescriptionEvasion.IntFromLineRegex();
            parserPatterns.ChanceToBlock = languageProvider.Language.DescriptionChanceToBlock.IntFromLineRegex();
            parserPatterns.Level = languageProvider.Language.DescriptionLevel.IntFromLineRegex();
            parserPatterns.AttacksPerSecond = languageProvider.Language.DescriptionAttacksPerSecond.DecimalFromLineRegex();
            parserPatterns.CriticalStrikeChance = languageProvider.Language.DescriptionCriticalStrikeChance.DecimalFromLineRegex();
            parserPatterns.ElementalDamage = languageProvider.Language.DescriptionElementalDamage.LineRegex();
            parserPatterns.PhysicalDamage = languageProvider.Language.DescriptionPhysicalDamage.LineRegex();

            parserPatterns.Quality = languageProvider.Language.DescriptionQuality.IntFromLineRegex();
            parserPatterns.AlternateQuality = languageProvider.Language.DescriptionAlternateQuality.ToRegex();

            parserPatterns.MapTier = languageProvider.Language.DescriptionMapTier.IntFromLineRegex();
            parserPatterns.ItemQuantity = languageProvider.Language.DescriptionItemQuantity.IntFromLineRegex();
            parserPatterns.ItemRarity = languageProvider.Language.DescriptionItemRarity.IntFromLineRegex();
            parserPatterns.MonsterPackSize = languageProvider.Language.DescriptionMonsterPackSize.IntFromLineRegex();
            parserPatterns.Blighted = languageProvider.Language.PrefixBlighted.ToRegex("[\\ \\r\\n]", "[\\ \\r\\n]");
        }

        #endregion Properties (Armour, Evasion, Energy Shield, Quality, Level)

        #region Sockets

        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            parserPatterns.Socket = languageProvider.Language.DescriptionSockets.ToRegex(suffix: "[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
        }

        #endregion Sockets

        #region Influences

        private void InitInfluences()
        {
            parserPatterns.Crusader = languageProvider.Language.InfluenceCrusader.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Elder = languageProvider.Language.InfluenceElder.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Hunter = languageProvider.Language.InfluenceHunter.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Redeemer = languageProvider.Language.InfluenceRedeemer.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Shaper = languageProvider.Language.InfluenceShaper.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Warlord = languageProvider.Language.InfluenceWarlord.ToRegex(prefix: "[\\r\\n]+");
        }

        #endregion Influences
    }
}
