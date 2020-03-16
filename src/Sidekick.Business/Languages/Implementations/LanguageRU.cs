using System;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("Russian", "Редкость: ")]
    public class LanguageRU : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://ru.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://ru.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://ru.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("https://ru.pathofexile.com/");
        public string RarityUnique => "Уникальный";
        public string RarityRare => "Редкий";
        public string RarityMagic => "Волшебный";
        public string RarityNormal => "Обычный";
        public string RarityCurrency => "Валюта";
        public string RarityGem => "Камень";
        public string RarityDivinationCard => "Гадальная карта";
        public string DescriptionUnidentified => "Неопознано";
        public string DescriptionQuality => "Качество: ";
        public string DescriptionCorrupted => "Осквернено";
        public string DescriptionRarity => "Редкость: ";
        public string DescriptionSockets => "Гнезда: ";
        public string DescriptionItemLevel => "Уровень предмета: ";
        public string PrefixSuperior => "Рог";
        public string InfluenceShaper => "Создателя";
        public string InfluenceElder => "Древнего";
        public string InfluenceCrusader => "Крестоносца";
        public string InfluenceHunter => "Охотника";
        public string InfluenceRedeemer => "Избавительницы";
        public string InfluenceWarlord => "Вождя";
        public string DescriptionMapTier => "Уровень карты: ";
        public string DescriptionItemQuantity => "Количество предметов: ";
        public string DescriptionItemRarity => "Редкость предметов: ";
        public string DescriptionMonsterPackSize => "Размер групп монстров: ";
        public string PrefixBlighted => "Заражённая";
        public string DescriptionExperience => "Опыт: ";
        public string DescriptionOrgan => "Использует: ";
        public string KeywordProphecy => "пророчество";
        public string KeywordVaal => "Ваал";
        public string KeywordCatalyst => "катализатор";
        public string KeywordOil => "масло";
        public string KeywordIncubator => "инкубатор";
        public string KeywordScarab => "скарабей";
        public string KeywordResonator => "резонатор";
        public string KeywordFossil => "ископаемое";
        public string KeywordVial => "Фиал";
        public string KeywordEssence => "сущность";

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
