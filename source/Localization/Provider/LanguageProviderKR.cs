using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderKR : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsKR.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsKR.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsKR.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsKR.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsKR.RarityUnique;
        public override string RarityRare => StringConstantsKR.RarityRare;
        public override string RarityMagic => StringConstantsKR.RarityMagic;
        public override string RarityNormal => StringConstantsKR.RarityNormal;
        public override string RarityCurrency => StringConstantsKR.RarityCurrency;
        public override string RarityGem => StringConstantsKR.RarityGem;
        public override string RarityDivinationCard => StringConstantsKR.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsKR.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsKR.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsKR.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsKR.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsKR.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsKR.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsKR.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsKR.InfluenceShaper;
        public override string InfluenceElder => StringConstantsKR.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsKR.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsKR.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsKR.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsKR.InfluenceWarlord;
    }
}
