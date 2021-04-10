using MediatR;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Maps.Models;

namespace Sidekick.Domain.Game.Maps.Commands
{
    /// <summary>
    /// Gets a map info object from an item
    /// </summary>
    public class GetMapInfo : IQuery<MapInfo>
    {
        /// <summary>
        /// Gets a map info object from an item
        /// </summary>
        /// <param name="item"></param>
        public GetMapInfo(Item item)
        {
            Item = item;
        }

        /// <summary>
        /// The map to get the info for
        /// </summary>
        public Item Item { get; }
    }
}
