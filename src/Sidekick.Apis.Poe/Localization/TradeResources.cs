using Microsoft.Extensions.Localization;

namespace Sidekick.Apis.Poe.Localization
{
    public class TradeResources
    {
        private readonly IStringLocalizer<TradeResources> localizer;

        public TradeResources(IStringLocalizer<TradeResources> localizer)
        {
            this.localizer = localizer;
        }

        public string Filters_Dps => localizer["Filters_Dps"];
        public string Filters_EDps => localizer["Filters_EDps"];
        public string Filters_PDps => localizer["Filters_PDps"];
    }
}
