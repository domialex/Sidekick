using System;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("TraditionalChinese", "稀有度: ")]
    public class LanguageZHTW : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("http://web.poe.garena.tw/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("http://web.poe.garena.tw/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("http://web.poe.garena.tw/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("http://web.poe.garena.tw/");
        public string RarityUnique => "傳奇";
        public string RarityRare => "稀有";
        public string RarityMagic => "魔法";
        public string RarityNormal => "普通";
        public string RarityCurrency => "通貨";
        public string RarityGem => "寶石";
        public string RarityDivinationCard => "命運卡";
        public string DescriptionUnidentified => "未鑑定";
        public string DescriptionQuality => "品質: ";
        public string DescriptionCorrupted => "已汙染";
        public string DescriptionRarity => "稀有度: ";
        public string DescriptionSockets => "插槽: ";
        public string DescriptionItemLevel => "物品等級: ";
        public string DescriptionExperience => "經驗值: ";
        public string DescriptionOrgan => "使用: ";
        public string PrefixSuperior => "精良的";
        public string InfluenceShaper => "塑者之物";
        public string InfluenceElder => "尊師之物";
        public string InfluenceCrusader => "聖戰軍王物品";
        public string InfluenceHunter => "狩獵者物品";
        public string InfluenceRedeemer => "救贖者物品";
        public string InfluenceWarlord => "總督軍物品";
        public string DescriptionMapTier => "地圖階級: ";
        public string DescriptionItemQuantity => "物品數量: ";
        public string DescriptionItemRarity => "物品稀有度: ";
        public string DescriptionMonsterPackSize => "怪物群大小: ";
        public string PrefixBlighted => "凋落的";
        public string KeywordProphecy => "預言";
        public string KeywordVaal => "瓦爾";
        public string KeywordCatalyst => "催化劑";
        public string KeywordOil => "油瓶";
        public string KeywordIncubator => "培育器";
        public string KeywordScarab => "聖甲蟲";
        public string KeywordResonator => "鑄新儀";
        public string KeywordFossil => "化石";
        public string KeywordVial => "之罈";
        public string KeywordEssence => "精髓";
    }
}
