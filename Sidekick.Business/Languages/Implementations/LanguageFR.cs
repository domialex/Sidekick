using System;

namespace Sidekick.Business.Languages.Implementations
{
    public class LanguageFR : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://fr.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://fr.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://fr.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://fr.pathofexile.com/");
        public string RarityUnique => "Unique";
        public string RarityRare => "Rare";
        public string RarityMagic => "Magique";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Objet monétaire";
        public string RarityGem => "Gemme";
        public string RarityDivinationCard => "Carte divinatoire";
        public string DescriptionUnidentified => "Non identifié";
        public string DescriptionQuality => "Qualité: ";
        public string DescriptionCorrupted => "Corrompu";
        public string DescriptionRarity => "Rareté: ";
        public string DescriptionSockets => "Châsses: ";
        public string DescriptionItemLevel => "Niveau de l'objet: ";
        public string DescriptionExperience => "Expérience: ";
        public string PrefixSuperior => "supérieure";
        public string InfluenceShaper => "Façonneur";
        public string InfluenceElder => "l'Ancien";
        public string InfluenceCrusader => "Croisé";
        public string InfluenceHunter => "Chasseur";
        public string InfluenceRedeemer => "la Rédemptrice";
        public string InfluenceWarlord => "Seigneur de guerre";
        public string DescriptionMapTier => "Palier de Carte: ";
        public string DescriptionItemQuantity => "Quantité d'objets: ";
        public string DescriptionItemRarity => "Rareté des objets: ";
        public string DescriptionMonsterPackSize => "Taille des groupes de monstres: ";
        public string PrefixBlighted => "infestée";
        public string KeywordProphecy => "prophétie";
        public string KeywordVaal => "Vaal";
        public string KeywordCatalyst => "Catalyseur";
        public string KeywordOil => "Huile";
        public string KeywordIncubator => "Incubateur";
        public string KeywordScarab => "Scarabée";
        public string KeywordResonator => "Résonateur";
        public string KeywordFossil => "Fossile";
        public string KeywordVial => "Fiole";
        public string KeywordEssence => "Essence";
    }
}
