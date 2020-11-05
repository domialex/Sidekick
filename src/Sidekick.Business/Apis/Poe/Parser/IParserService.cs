using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public interface IParserService
    {
        Item ParseItem(string itemText);
    }
}
