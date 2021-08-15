using System.Threading.Tasks;
using Sidekick.Apis.Poe.Parser;
using Sidekick.Common.Game.Items;

namespace Sidekick.Apis.Poe
{
    public interface IItemMetadataProvider
    {
        Task Initialize();

        ItemMetadata Parse(ParsingItem parsingItem);
    }
}
