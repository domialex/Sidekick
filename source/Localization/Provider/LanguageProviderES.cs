using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderES : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsES.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsES.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsES.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsES.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsES.RarityUnique;
        public override string RarityRare => StringConstantsES.RarityRare;
        public override string RarityMagic => StringConstantsES.RarityMagic;
        public override string RarityNormal => StringConstantsES.RarityNormal;
        public override string RarityCurrency => StringConstantsES.RarityCurrency;
        public override string RarityGem => StringConstantsES.RarityGem;
        public override string RarityDivinationCard => StringConstantsES.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsES.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsES.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsES.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsES.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsES.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsES.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsES.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsES.InfluenceShaper;
        public override string InfluenceElder => StringConstantsES.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsES.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsES.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsES.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsES.InfluenceWarlord;
    }
}
