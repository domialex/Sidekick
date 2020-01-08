using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderRU : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsRU.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsRU.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsRU.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsRU.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsRU.RarityUnique;
        public override string RarityRare => StringConstantsRU.RarityRare;
        public override string RarityMagic => StringConstantsRU.RarityMagic;
        public override string RarityNormal => StringConstantsRU.RarityNormal;
        public override string RarityCurrency => StringConstantsRU.RarityCurrency;
        public override string RarityGem => StringConstantsRU.RarityGem;
        public override string RarityDivinationCard => StringConstantsRU.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsRU.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsRU.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsRU.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsRU.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsRU.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsRU.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsRU.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsRU.InfluenceShaper;
        public override string InfluenceElder => StringConstantsRU.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsRU.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsRU.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsRU.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsRU.InfluenceWarlord;
    }
}
