using Sidekick.Localization.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization.Provider
{
    public class LanguageProviderEN : LanguageProvider
    {
        public override Uri PoeTradeSearchBaseUrl => new Uri(StringConstantsEN.PoeTradeSearchBaseUrl);
        public override Uri PoeTradeExchangeBaseUrl => new Uri(StringConstantsEN.PoeTradeExchangeBaseUrl);
        public override Uri PoeTradeApiBaseUrl => new Uri(StringConstantsEN.PoeTradeApiBaseUrl);
        public override Uri PoeCdnBaseUrl => new Uri(StringConstantsEN.PoeCdnBaseUrl);
        public override string RarityUnique => StringConstantsEN.RarityUnique;
        public override string RarityRare => StringConstantsEN.RarityRare;
        public override string RarityMagic => StringConstantsEN.RarityMagic;
        public override string RarityNormal => StringConstantsEN.RarityNormal;
        public override string RarityCurrency => StringConstantsEN.RarityCurrency;
        public override string RarityGem => StringConstantsEN.RarityGem;
        public override string RarityDivinationCard => StringConstantsEN.RarityDivinationCard;
        public override string DescriptionUnidentified => StringConstantsEN.DescriptionUnidentified;
        public override string DescriptionQuality => StringConstantsEN.DescriptionQuality;
        public override string DescriptionCorrupted => StringConstantsEN.DescriptionCorrupted;
        public override string DescriptionRarity => StringConstantsEN.DescriptionRarity;
        public override string DescriptionSockets => StringConstantsEN.DescriptionSockets;
        public override string DescriptionItemLevel => StringConstantsEN.DescriptionItemLevel;
        public override string PrefixSuperior => StringConstantsEN.PrefixSuperior;
        public override string InfluenceShaper => StringConstantsEN.InfluenceShaper;
        public override string InfluenceElder => StringConstantsEN.InfluenceElder;
        public override string InfluenceCrusader => StringConstantsEN.InfluenceCrusader;
        public override string InfluenceHunter => StringConstantsEN.InfluenceHunter;
        public override string InfluenceRedeemer => StringConstantsEN.InfluenceRedeemer;
        public override string InfluenceWarlord => StringConstantsEN.InfluenceWarlord;
    }
}
