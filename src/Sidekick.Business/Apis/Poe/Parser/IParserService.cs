using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public interface IParserService
    {
        Item ParseItem(string itemText);
    }
}
