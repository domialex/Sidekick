using System;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("German", "Seltenheit: ")]
    public class LanguageDE : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://de.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://de.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://de.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("https://de.pathofexile.com/");
        public string RarityUnique => "Einzigartig";
        public string RarityRare => "Selten";
        public string RarityMagic => "Magisch";
        public string RarityNormal => "Normal";
        public string RarityCurrency => "Währung";
        public string RarityGem => "Gemme";
        public string RarityDivinationCard => "Weissagungskarte";
        public string DescriptionUnidentified => "Nicht identifiziert";
        public string DescriptionQuality => "Qualität: ";
        public string DescriptionCorrupted => "Verderbt";
        public string DescriptionRarity => "Seltenheit: ";
        public string DescriptionSockets => "Fassungen: ";
        public string DescriptionItemLevel => "Gegenstandsstufe: ";
        public string DescriptionExperience => "Erfahrung: ";
        public string DescriptionOrgan => "Verwendet: ";
        public string PrefixSuperior => "(hochwertig)";
        public string InfluenceShaper => "Schöpfer";
        public string InfluenceElder => "Ältesten";
        public string InfluenceCrusader => "Kreuzritter";
        public string InfluenceHunter => "Jägers";
        public string InfluenceRedeemer => "Erlöserin";
        public string InfluenceWarlord => "Kriegsherrn";
        public string DescriptionMapTier => "Kartenlevel: ";
        public string DescriptionItemQuantity => "Gegenstandsmenge: ";
        public string DescriptionItemRarity => "Gegenstandsseltenheit: ";
        public string DescriptionMonsterPackSize => "Monstergruppengröße: ";
        public string PrefixBlighted => "Befallene";
        public string KeywordProphecy => "Prophezeiung";
        public string KeywordVaal => "Vaal";
        public string KeywordCatalyst => "Katalysator";
        public string KeywordOil => "Öl";
        public string KeywordIncubator => "Inkubator";
        public string KeywordScarab => "Skarabäus";
        public string KeywordResonator => "Resonator";
        public string KeywordFossil => "Fossil";
        public string KeywordVial => "Phiole";
        public string KeywordEssence => "Essenz";

        public string AttributeCategoryCrafted => "Angefertigt";
        public string PercentagAddedRegexPattern => "^[+]+[\\d{1,2}]+%";
        public string PercentageIncreasedOrDecreasedRegexPattern => "[-+]?[\\d{1,3}]+%";
        public string AttributeIncreasedRegexPattern => "[+]?[\\d{1,3}]+[\\s]";
        public string CategoryNameCrafted => "(crafted)";
        public string AttributeCategoryImplicit => "Implizit";
        public string CategoryNameImplicit => "(implicit)";
        public string AttributeCategoryFractured => "Brüchig";
        public string CategoryNameFractured => "(fractured)";
        public string AttributeCategoryEnchant => "Verzauberung";
        public string CategoryNameEnchant => "(enchant)";
        public string AttributeCategoryVeiled => "Verhüllt";
        public string CategoryNameVeiled => "(veiled)";
        public string AttributeCategoryDelve => "Delve";
        public string CategoryNameDelve => "(delve)";
        public string AttributeCategoryExplicit => "Explizit";
        public string AttributeRangeRegexPattern => "[\\d{1,3}]+[\\s]+[\\bto\\b]+[\\s]+[\\d{1,3}]+[\\s]";
        public string KeywordRange => "bis";
        public string DescriptionPhysicalDamage => "Physischer Schaden: ";
        public string DescriptionElementalDamage => "Elementarschaden: ";
        public string DescriptionAttacksPerSecond => "Angriffe pro Sekunde: ";
        public string DescriptionCriticalStrikeChance => "Kritische Trefferchance: ";
        public string DescriptionEnergyShield => "Energieschild: ";
        public string DescriptionArmour => "Rüstung: ";
        public string DescriptionEvasion => "Ausweichwert: ";
    }
}
