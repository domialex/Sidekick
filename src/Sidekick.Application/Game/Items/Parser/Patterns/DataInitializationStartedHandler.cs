using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Application.Game.Items.Parser.Extensions;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Application.Game.Items.Parser.Patterns
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly IParserPatterns parserPatterns;

        public DataInitializationStartedHandler(
            IGameLanguageProvider gameLanguageProvider,
            IParserPatterns parserPatterns)
        {
            this.gameLanguageProvider = gameLanguageProvider;
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
                { Rarity.Normal, gameLanguageProvider.Language.RarityNormal.EndOfLineRegex() },
                { Rarity.Magic, gameLanguageProvider.Language.RarityMagic.EndOfLineRegex() },
                { Rarity.Rare, gameLanguageProvider.Language.RarityRare.EndOfLineRegex() },
                { Rarity.Unique, gameLanguageProvider.Language.RarityUnique.EndOfLineRegex() },
                { Rarity.Currency, gameLanguageProvider.Language.RarityCurrency.EndOfLineRegex() },
                { Rarity.Gem, gameLanguageProvider.Language.RarityGem.EndOfLineRegex() },
                { Rarity.DivinationCard, gameLanguageProvider.Language.RarityDivinationCard.EndOfLineRegex() }
            };

            parserPatterns.ItemLevel = gameLanguageProvider.Language.DescriptionItemLevel.IntFromLineRegex();
            parserPatterns.Unidentified = gameLanguageProvider.Language.DescriptionUnidentified.ToRegex(prefix: "[\\r\\n]");
            parserPatterns.Corrupted = gameLanguageProvider.Language.DescriptionCorrupted.ToRegex(prefix: "[\\r\\n]");
        }

        #endregion Header (Rarity, Name, Type)

        #region Properties (Armour, Evasion, Energy Shield, Quality, Level)

        private void InitProperties()
        {
            parserPatterns.Armor = gameLanguageProvider.Language.DescriptionArmour.ToRegex(prefix: "(?:^|[\\r\\n])", suffix: "[^\\r\\n\\d]*(\\d+)");

            parserPatterns.EnergyShield = gameLanguageProvider.Language.DescriptionEnergyShield.IntFromLineRegex();
            parserPatterns.Evasion = gameLanguageProvider.Language.DescriptionEvasion.IntFromLineRegex();
            parserPatterns.ChanceToBlock = gameLanguageProvider.Language.DescriptionChanceToBlock.IntFromLineRegex();
            parserPatterns.Level = gameLanguageProvider.Language.DescriptionLevel.IntFromLineRegex();
            parserPatterns.AttacksPerSecond = gameLanguageProvider.Language.DescriptionAttacksPerSecond.DecimalFromLineRegex();
            parserPatterns.CriticalStrikeChance = gameLanguageProvider.Language.DescriptionCriticalStrikeChance.DecimalFromLineRegex();
            parserPatterns.ElementalDamage = gameLanguageProvider.Language.DescriptionElementalDamage.LineRegex();
            parserPatterns.PhysicalDamage = gameLanguageProvider.Language.DescriptionPhysicalDamage.LineRegex();

            parserPatterns.Quality = gameLanguageProvider.Language.DescriptionQuality.IntFromLineRegex();
            parserPatterns.AlternateQuality = gameLanguageProvider.Language.DescriptionAlternateQuality.ToRegex();

            parserPatterns.MapTier = gameLanguageProvider.Language.DescriptionMapTier.IntFromLineRegex();
            parserPatterns.ItemQuantity = gameLanguageProvider.Language.DescriptionItemQuantity.IntFromLineRegex();
            parserPatterns.ItemRarity = gameLanguageProvider.Language.DescriptionItemRarity.IntFromLineRegex();
            parserPatterns.MonsterPackSize = gameLanguageProvider.Language.DescriptionMonsterPackSize.IntFromLineRegex();
            parserPatterns.Blighted = gameLanguageProvider.Language.PrefixBlighted.ToRegex("[\\ \\r\\n]", "[\\ \\r\\n]");
        }

        #endregion Properties (Armour, Evasion, Energy Shield, Quality, Level)

        #region Sockets

        private void InitSockets()
        {
            // We need 6 capturing groups as it is possible for a 6 socket unlinked item to exist
            parserPatterns.Socket = gameLanguageProvider.Language.DescriptionSockets.ToRegex(suffix: "[^\\r\\n]*?([-RGBWA]+)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)\\ ?([-RGBWA]*)");
        }

        #endregion Sockets

        #region Influences

        private void InitInfluences()
        {
            parserPatterns.Crusader = gameLanguageProvider.Language.InfluenceCrusader.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Elder = gameLanguageProvider.Language.InfluenceElder.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Hunter = gameLanguageProvider.Language.InfluenceHunter.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Redeemer = gameLanguageProvider.Language.InfluenceRedeemer.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Shaper = gameLanguageProvider.Language.InfluenceShaper.ToRegex(prefix: "[\\r\\n]+");
            parserPatterns.Warlord = gameLanguageProvider.Language.InfluenceWarlord.ToRegex(prefix: "[\\r\\n]+");
        }

        #endregion Influences
    }
}
