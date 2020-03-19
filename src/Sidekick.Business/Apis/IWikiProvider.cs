using Sidekick.Business.Apis.Poe.Parser;

namespace Sidekick.Business.Apis
{
    public interface IWikiProvider
    {
        void Open(ParsedItem item);
    }
}
