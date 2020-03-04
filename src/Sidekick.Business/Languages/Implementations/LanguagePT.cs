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

        public string AttributeCategoryCrafted => throw new NotImplementedException();

        public Regex PercentagAddedRegex => throw new NotImplementedException();

        public string PercentagAddedRegexPattern => throw new NotImplementedException();

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
