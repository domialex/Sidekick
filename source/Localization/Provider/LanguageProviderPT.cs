using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderPT : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsPT.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsPT.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsPT.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsPT.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsPT.RarityUnique;
        public override string RarityRare => StringConstantsPT.RarityRare;
        public override string RarityMagic => StringConstantsPT.RarityMagic;
        public override string RarityNormal => StringConstantsPT.RarityNormal;
        public override string RarityCurrency => StringConstantsPT.RarityCurrency;
        public override string RarityGem => StringConstantsPT.RarityGem;
        public override string RarityDivinationCard => StringConstantsPT.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsPT.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsPT.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsPT.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsPT.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsPT.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsPT.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsPT.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsPT.InfluenceShaper;
        public override string InfluenceElder => StringConstantsPT.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsPT.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsPT.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsPT.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsPT.InfluenceWarlord;
    }
}
