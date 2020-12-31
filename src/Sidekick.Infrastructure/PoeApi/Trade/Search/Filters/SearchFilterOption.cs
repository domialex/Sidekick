using Sidekick.Domain.Game.Trade.Models;

namespace Sidekick.Infrastructure.PoeApi.Trade.Search.Filters
{
    public class SearchFilterOption
    {
        public SearchFilterOption(string option)
        {
            Option = option;
        }

        public SearchFilterOption(PropertyFilter filter)
        {
            Option = filter.Enabled ? "true" : "false";
        }

        public string Option { get; set; }
    }
}
