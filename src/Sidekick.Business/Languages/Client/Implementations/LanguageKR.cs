using System;

namespace Sidekick.Business.Languages.Client.Implementations
{
    [Language("Korean", "아이템 희귀도: ")]
    public class LanguageKR : ILanguage
    {
        public Uri PoeTradeSearchBaseUrl => new Uri("https://poe.game.daum.net/trade/search/");
        public Uri PoeTradeExchangeBaseUrl => new Uri("https://poe.game.daum.net/trade/exchange/");
        public Uri PoeTradeApiBaseUrl => new Uri("https://poe.game.daum.net/api/trade/");
        public Uri PoeCdnBaseUrl => new Uri("https://web.poecdn.com/");
        public Uri PoeWebsite => new Uri("https://poe.game.daum.net/");
        public string RarityUnique => "고유";
        public string RarityRare => "희귀";
        public string RarityMagic => "마법";
        public string RarityNormal => "일반";
        public string RarityCurrency => "화폐";
        public string RarityGem => "젬";
        public string RarityDivinationCard => "점술 카드";
        public string DescriptionUnidentified => "미확인";
        public string DescriptionQuality => "퀄리티: ";
        public string DescriptionCorrupted => "타락";
        public string DescriptionRarity => "아이템 희귀도: ";
        public string DescriptionSockets => "홈: ";
        public string DescriptionItemLevel => "아이템 레벨: ";
        public string PrefixSuperior => "상";
        public string InfluenceShaper => "쉐이퍼";
        public string InfluenceElder => "엘더";
        public string InfluenceCrusader => "성전사";
        public string InfluenceHunter => "사냥꾼";
        public string InfluenceRedeemer => "대속자";
        public string InfluenceWarlord => "전쟁군주";
        public string DescriptionMapTier => "지도 등급: ";
        public string DescriptionItemQuantity => "아이템 수량: ";
        public string DescriptionItemRarity => "아이템 희귀도: ";
        public string DescriptionMonsterPackSize => "몬스터 무리 규모: ";
        public string PrefixBlighted => "역병";
        public string DescriptionExperience => "경험치: ";
        public string DescriptionOrgan => "사용: ";
        public string KeywordProphecy => "예언을";
        public string KeywordVaal => "바알";
        public string KeywordCatalyst => "기폭제";
        public string KeywordOil => "성유";
        public string KeywordIncubator => "인큐베이터";
        public string KeywordScarab => "갑충석";
        public string KeywordResonator => "공명기";
        public string KeywordFossil => "화석";
        public string KeywordVial => "시약";
        public string KeywordEssence => "에센스";
    }
}
