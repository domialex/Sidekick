using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderFR : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsFR.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsFR.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsFR.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsFR.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsFR.RarityUnique;
        public override string RarityRare => StringConstantsFR.RarityRare;
        public override string RarityMagic => StringConstantsFR.RarityMagic;
        public override string RarityNormal => StringConstantsFR.RarityNormal;
        public override string RarityCurrency => StringConstantsFR.RarityCurrency;
        public override string RarityGem => StringConstantsFR.RarityGem;
        public override string RarityDivinationCard => StringConstantsFR.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsFR.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsFR.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsFR.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsFR.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsFR.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsFR.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsFR.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsFR.InfluenceShaper;
        public override string InfluenceElder => StringConstantsFR.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsFR.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsFR.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsFR.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsFR.InfluenceWarlord;
    }
}
