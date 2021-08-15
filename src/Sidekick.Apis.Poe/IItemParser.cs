using System.Threading.Tasks;
using Sidekick.Apis.Poe.Parser;
using Sidekick.Common.Game.Items;

namespace Sidekick.Apis.Poe
{
    public interface IItemParser
    {
        Task Initialize();

        ParsingItem GetParsingItem(string itemText);

        Item ParseItem(string itemText);
    }
}
