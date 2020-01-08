using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Localization
{
    public abstract class LanguageProvider
    {
        public abstract Uri PoeTradeSearchBaseUrl { get; }
        public abstract Uri PoeTradeExchangeBaseUrl { get; }
        public abstract Uri PoeTradeApiBaseUrl { get; }
        public abstract Uri PoeCdnBaseUrl { get; }

        public abstract string RarityUnique { get; }
        public abstract string RarityRare { get; }
        public abstract string RarityMagic { get; }
        public abstract string RarityNormal { get; }
        public abstract string RarityCurrency { get; }
        public abstract string RarityGem { get; }
        public abstract string RarityDivinationCard { get; }

        public abstract string DescriptionUnidentified { get; }
        public abstract string DescriptionQuality { get; }
        public abstract string DescriptionCorrupted { get; }
        public abstract string DescriptionRarity { get; }
        public abstract string DescriptionSockets { get; }
        public abstract string DescriptionItemLevel { get; }

        public abstract string PrefixSuperior { get; }

        public abstract string InfluenceShaper { get; }
        public abstract string InfluenceElder { get; }
        public abstract string InfluenceCrusader { get; }
        public abstract string InfluenceHunter { get; }
        public abstract string InfluenceRedeemer { get; }
        public abstract string InfluenceWarlord { get; }
    }
}
