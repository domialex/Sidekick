using System;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("Portuguese", "Raridade: ")]
    public class LanguagePT : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://br.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://br.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://br.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("https://br.pathofexile.com/");
        public string RarityUnique => "Único";
        public string RarityRare => "Raro";
        public string RarityMagic => "Mágico";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Moeda";
        public string RarityGem => "Gema";
        public string RarityDivinationCard => "Carta de Adivinhação";
        public string DescriptionUnidentified => "Não Identificado";
        public string DescriptionQuality => "Qualidade: ";
        public string DescriptionCorrupted => "Corrompido";
        public string DescriptionRarity => "Raridade: ";
        public string DescriptionSockets => "Encaixes: ";
        public string DescriptionItemLevel => "Nível do Item: ";
        public string DescriptionExperience => "Experiência: ";
        public string DescriptionOrgan => "Usos: ";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "Criador";
        public string InfluenceElder => "Ancião";
        public string InfluenceCrusader => "Cruzado";
        public string InfluenceHunter => "Caçador";
        public string InfluenceRedeemer => "Redentor";
        public string InfluenceWarlord => "Senhor da Guerra";
        public string DescriptionMapTier => "Tier do Mapa: ";
        public string DescriptionItemQuantity => "Quantidade de Itens: ";
        public string DescriptionItemRarity => "Raridade de Itens: ";
        public string DescriptionMonsterPackSize => "Tamanho do Grupo de Monstros: ";
        public string PrefixBlighted => "Arruinado";
        public string KeywordProphecy => "profecia";
        public string KeywordVaal => "Vaal";
        public string KeywordCatalyst => "Catalisador";
        public string KeywordOil => "Óleo";
        public string KeywordIncubator => "Incubador";
        public string KeywordScarab => "Escaravelho";
        public string KeywordResonator => "Ressonador";
        public string KeywordFossil => "Fóssil";
        public string KeywordVial => "Recipiente";
        public string KeywordEssence => "Essência";

        public string AttributeCategoryCrafted => "__TranslationRequired__";

        public Regex PercentagAddedRegex => throw new NotSupportedException();

        public string PercentageAddedRegexPattern => "__TranslationRequired__";

        public string PercentageIncreasedOrDecreasedRegexPattern => "__TranslationRequired__";

        public string AttributeIncreasedRegexPattern => "__TranslationRequired__";

        public string CategoryNameCrafted => "__TranslationRequired__";

        public string AttributeCategoryImplicit => "__TranslationRequired__";

        public string CategoryNameImplicit => "__TranslationRequired__";

        public string AttributeCategoryFractured => "__TranslationRequired__";

        public string CategoryNameFractured => "__TranslationRequired__";

        public string AttributeCategoryEnchant => "__TranslationRequired__";

        public string CategoryNameEnchant => "__TranslationRequired__";

        public string AttributeCategoryVeiled => "__TranslationRequired__";

        public string CategoryNameVeiled => "__TranslationRequired__";

        public string AttributeCategoryDelve => "__TranslationRequired__";

        public string CategoryNameDelve => "__TranslationRequired__";

        public string AttributeCategoryExplicit => "__TranslationRequired__";

        public string AttributeRangeRegexPattern => "__TranslationRequired__";

        public string KeywordRange => "__TranslationRequired__";

        public string DescriptionPhysicalDamage => "__TranslationRequired__";

        public string DescriptionElementalDamage => "__TranslationRequired__";

        public string DescriptionAttacksPerSecond => "__TranslationRequired__";

        public string DescriptionCriticalStrikeChance => "__TranslationRequired__";

        public string DescriptionEnergyShield => "__TranslationRequired__";

        public string DescriptionArmour => "__TranslationRequired__";

        public string DescriptionEvasion => "__TranslationRequired__";

        public string DescriptionLevel => "__TranslationRequired__";
    }
}
