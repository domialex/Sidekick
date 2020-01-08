using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderTH : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsTH.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsTH.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsTH.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsTH.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsTH.RarityUnique;
        public override string RarityRare => StringConstantsTH.RarityRare;
        public override string RarityMagic => StringConstantsTH.RarityMagic;
        public override string RarityNormal => StringConstantsTH.RarityNormal;
        public override string RarityCurrency => StringConstantsTH.RarityCurrency;
        public override string RarityGem => StringConstantsTH.RarityGem;
        public override string RarityDivinationCard => StringConstantsTH.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsTH.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsTH.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsTH.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsTH.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsTH.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsTH.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsTH.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsTH.InfluenceShaper;
        public override string InfluenceElder => StringConstantsTH.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsTH.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsTH.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsTH.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsTH.InfluenceWarlord;
    }
}
