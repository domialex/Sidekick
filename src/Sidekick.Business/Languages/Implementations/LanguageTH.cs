using System;

namespace Sidekick.Business.Languages.Implementations
{
    [Language("Thai", "ความหายาก", "th")]
    public class LanguageTH : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://th.pathofexile.com/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://th.pathofexile.com/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://th.pathofexile.com/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("https://th.pathofexile.com/");
        public string RarityUnique => "Unique";
        public string RarityRare => "แรร์";
        public string RarityMagic => "เมจิก";
        public string RarityNormal => "ปกติ";
        public string RarityCurrency => "เคอเรนซี่";
        public string RarityGem => "เจ็ม";
        public string RarityDivinationCard => "ไพ่พยากรณ์";
        public string DescriptionUnidentified => "ยังไม่ได้ตรวจสอบ";
        public string DescriptionQuality => "คุณภาพ";
        public string DescriptionCorrupted => "คอร์รัปต์";
        public string DescriptionRarity => "ความหายาก";
        public string DescriptionSockets => "ซ็อกเก็ต";
        public string DescriptionItemLevel => "เลเวลไอเทม";
        public string PrefixSuperior => "Superior";
        public string InfluenceShaper => "เชปเปอร์";
        public string InfluenceElder => "เอลเดอร์";
        public string InfluenceCrusader => "ครูเซเดอร์";
        public string InfluenceHunter => "ฮันเตอร์";
        public string InfluenceRedeemer => "รีดีมเมอร์";
        public string InfluenceWarlord => "วอร์หลอด";
        public string DescriptionMapTier => "ระดับแผนที่";
        public string DescriptionItemQuantity => "จำนวนไอเท็ม";
        public string DescriptionItemRarity => "ระดับความหายากของไอเทม";
        public string DescriptionMonsterPackSize => "ขนาดบรรจุมอนสเตอร์";
        public string PrefixBlighted => "Blighted";
        public string DescriptionExperience => "ประสบการณ์";
        public string DescriptionOrgan => "ใช้";
        public string KeywordProphecy => "(prophecy)";
        public string KeywordVaal => "วาล์";
        public string KeywordCatalyst => "Catalyst";
        public string KeywordOil => "Oil";
        public string KeywordIncubator => "Incubator";
        public string KeywordScarab => "Scarab";
        public string KeywordResonator => "Resonator";
        public string KeywordFossil => "Fossil";
        public string KeywordVial => "Vial";
        public string KeywordEssence => "Essence";

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
    }
}
