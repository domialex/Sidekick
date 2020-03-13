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
