using System.Collections.Generic;
using Sidekick.Domain.Game.Items.Models;

namespace Sidekick.Domain.Game.Maps.Models
{
    /// <summary>
    /// Contains map information
    /// </summary>
    public class MapInfo
    {
        /// <summary>
        /// The item this map represents
        /// </summary>
        public Item Item { get; set; }

        /// <summary>
        /// Modifiers that are not okay to run. Configurable through settings
        /// </summary>
        public List<string> DangerousMods { get; set; }

        /// <summary>
        /// Represents modifiers that are okay to run
        /// </summary>
        public List<string> OkMods { get; set; }
    }
}
