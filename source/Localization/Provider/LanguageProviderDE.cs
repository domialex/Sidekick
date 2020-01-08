using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderDE : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsDE.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsDE.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsDE.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsDE.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsDE.RarityUnique;
        public override string RarityRare => StringConstantsDE.RarityRare;
        public override string RarityMagic => StringConstantsDE.RarityMagic;
        public override string RarityNormal => StringConstantsDE.RarityNormal;
        public override string RarityCurrency => StringConstantsDE.RarityCurrency;
        public override string RarityGem => StringConstantsDE.RarityGem;
        public override string RarityDivinationCard => StringConstantsDE.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsDE.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsDE.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsDE.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsDE.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsDE.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsDE.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsDE.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsDE.InfluenceShaper;
        public override string InfluenceElder => StringConstantsDE.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsDE.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsDE.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsDE.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsDE.InfluenceWarlord;
    }
}
