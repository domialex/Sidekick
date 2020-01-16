namespace Sidekick.Business.Filters
{
    public class SocketFilter
    {
        public bool Disabled { get; set; }
        public SocketFilters Filters { get; set; } = new SocketFilters();
    }
}
