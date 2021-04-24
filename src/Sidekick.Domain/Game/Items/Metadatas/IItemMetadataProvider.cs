using System.Threading.Tasks;
using Sidekick.Domain.Game.Items.Metadatas.Models;

namespace Sidekick.Domain.Game.Items.Metadatas
{
    public interface IItemMetadataProvider
    {
        Task Initialize();
        ItemMetadata Parse(ParsingItem parsingItem);

    }
}
