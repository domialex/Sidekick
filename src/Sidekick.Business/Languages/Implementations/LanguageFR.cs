using System;
using Sidekick.Domain.Languages;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("French", "Rareté", "fr")]
    public class LanguageFR : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://fr.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://fr.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://fr.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public string RarityUnique => "Unique";
        public string RarityRare => "Rare";
        public string RarityMagic => "Magique";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Objet monétaire";
        public string RarityGem => "Gemme";
        public string RarityDivinationCard => "Carte divinatoire";
        public string DescriptionUnidentified => "Non identifié";
        public string DescriptionQuality => "Qualité";
        public string DescriptionAlternateQuality => "Qualité alternative";
        public string DescriptionCorrupted => "Corrompu";
        public string DescriptionRarity => "Rareté";
        public string DescriptionSockets => "Châsses";
        public string DescriptionItemLevel => "Niveau de l'objet";
        public string DescriptionExperience => "Expérience";
        public string DescriptionOrgan => "Aptitude ";
        public string PrefixSuperior => "supérieure";
        public string InfluenceShaper => "Façonneur";
        public string InfluenceElder => "l'Ancien";
        public string InfluenceCrusader => "Croisé";
        public string InfluenceHunter => "Chasseur";
        public string InfluenceRedeemer => "la Rédemptrice";
        public string InfluenceWarlord => "Seigneur de guerre";
        public string DescriptionMapTier => "Palier de Carte";
        public string DescriptionItemQuantity => "Quantité d'objets";
        public string DescriptionItemRarity => "Rareté des objets";
        public string DescriptionMonsterPackSize => "Taille des groupes de monstres";
        public string PrefixBlighted => "infestée";
        public string KeywordVaal => "Vaal";

        public string DescriptionPhysicalDamage => "__TranslationRequired__";

        public string DescriptionElementalDamage => "__TranslationRequired__";

        public string DescriptionAttacksPerSecond => "__TranslationRequired__";

        public string DescriptionCriticalStrikeChance => "__TranslationRequired__";

        public string DescriptionEnergyShield => "__TranslationRequired__";

        public string DescriptionArmour => "__TranslationRequired__";

        public string DescriptionEvasion => "__TranslationRequired__";
        public string DescriptionChanceToBlock => "__TranslationRequired__";

        public string DescriptionLevel => "__TranslationRequired__";

        public string ModifierIncreased => "__TranslationRequired__";
        public string ModifierReduced => "__TranslationRequired__";
        public string AdditionalProjectile => "tire un Projectile supplémentaire";
        public string AdditionalProjectiles => "tire # Projectiles supplémentaires";

        public string PrefixAnomalous => "anormale";
        public string PrefixDivergent => "divergente";
        public string PrefixPhantasmal => "fantasmatique";
    }
}
