using Sidekick.Domain.Game.Trade.Models;

namespace Sidekick.Infrastructure.PoeApi.Trade.Filters
{
    public class SearchFilterValue
    {
        internal SearchFilterValue() { }

        public SearchFilterValue(double? min, double? max)
        {
            Min = min;
            Max = max;
        }

        public SearchFilterValue(PropertyFilter filter)
        {
            Option = filter.Enabled ? "true" : "false";
            Min = filter.Min;
            Max = filter.Max;
        }

        public SearchFilterValue(ModifierFilter filter)
        {
            Option = filter.Modifier.OptionValue;
            Min = filter.Min;
            Max = filter.Max;
        }

        public object Option { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
    }
}
