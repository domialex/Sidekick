using System;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("Spanish", "Rareza: ")]
    public class LanguageES : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://es.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://es.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://es.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("https://es.pathofexile.com/");
        public string RarityUnique => "Único";
        public string RarityRare => "Raro";
        public string RarityMagic => "Mágico";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Objetos Monetarios";
        public string RarityGem => "Gema";
        public string RarityDivinationCard => "Carta de Adivinación";
        public string DescriptionUnidentified => "Sin identificar";
        public string DescriptionQuality => "Calidad: ";
        public string DescriptionCorrupted => "Corrupto";
        public string DescriptionRarity => "Rareza: ";
        public string DescriptionSockets => "Engarces: ";
        public string DescriptionItemLevel => "Nivel de Objeto: ";
        public string DescriptionExperience => "Experiencia: ";
        public string DescriptionOrgan => "Usa: ";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "Creador";
        public string InfluenceElder => "Antiguo";
        public string InfluenceCrusader => "Cruzado";
        public string InfluenceHunter => "Cazador";
        public string InfluenceRedeemer => "Redentora";
        public string InfluenceWarlord => "Jefe de guerra";
        public string DescriptionMapTier => "Grado del Mapa: ";
        public string DescriptionItemQuantity => "Cantidad de Objetos: ";
        public string DescriptionItemRarity => "Rareza de Objetos: ";
        public string DescriptionMonsterPackSize => "Tamaño de Grupos de Monstruos: ";
        public string PrefixBlighted => "Infestado";
        public string KeywordProphecy => "profecía";
        public string KeywordVaal => "Vaal";
        public string KeywordCatalyst => "Catalizador";
        public string KeywordOil => "Aceite";
        public string KeywordIncubator => "Incubadora";
        public string KeywordScarab => "Escarabajo";
        public string KeywordResonator => "Resonador";
        public string KeywordFossil => "Fósil";
        public string KeywordVial => "Vial";
        public string KeywordEssence => "Esencia";

        public string AttributeCategoryCrafted => throw new NotImplementedException();

        public Regex PercentagAddedRegex => throw new NotImplementedException();

        public string PercentageAddedRegexPattern => throw new NotImplementedException();

        public string PercentageIncreasedOrDecreasedRegexPattern => throw new NotImplementedException();

        public string AttributeIncreasedRegexPattern => throw new NotImplementedException();

        public string CategoryNameCrafted => throw new NotImplementedException();

        public string AttributeCategoryImplicit => throw new NotImplementedException();

        public string CategoryNameImplicit => throw new NotImplementedException();

        public string AttributeCategoryFractured => throw new NotImplementedException();

        public string CategoryNameFractured => throw new NotImplementedException();

        public string AttributeCategoryEnchant => throw new NotImplementedException();

        public string CategoryNameEnchant => throw new NotImplementedException();

        public string AttributeCategoryVeiled => throw new NotImplementedException();

        public string CategoryNameVeiled => throw new NotImplementedException();

        public string AttributeCategoryDelve => throw new NotImplementedException();

        public string CategoryNameDelve => throw new NotImplementedException();

        public string AttributeCategoryExplicit => throw new NotImplementedException();

        public string AttributeRangeRegexPattern => throw new NotImplementedException();

        public string KeywordRange => throw new NotImplementedException();

        public string DescriptionPhysicalDamage => throw new NotImplementedException();

        public string DescriptionElementalDamage => throw new NotImplementedException();

        public string DescriptionAttacksPerSecond => throw new NotImplementedException();

        public string DescriptionCriticalStrikeChance => throw new NotImplementedException();

        public string DescriptionEnergyShield => throw new NotImplementedException();

        public string DescriptionArmour => throw new NotImplementedException();

        public string DescriptionEvasion => throw new NotImplementedException();
    }
}
