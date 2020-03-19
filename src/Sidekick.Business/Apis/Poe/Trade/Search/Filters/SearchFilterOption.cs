namespace Sidekick.Business.Apis.Poe.Trade.Search.Filters
{
    public class SearchFilterOption
    {
        public SearchFilterOption() { }
        public SearchFilterOption(string option)
        {
            Option = option;
        }

        public string Option { get; set; }
    }
}
