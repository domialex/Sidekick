using MediatR;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Trade.Models;

namespace Sidekick.Domain.Game.Trade.Commands
{
    /// <summary>
    /// Save filter settings
    /// </summary>
    public class SaveFilterSettings : ICommand
    {
        /// <summary>
        /// Save filter settings
        /// </summary>
        /// <param name="item">The item for which to save modifiers for</param>
        /// <param name="modifierFilters">The modifiers to save</param>
        public SaveFilterSettings(Item item, ModifierFilters modifierFilters)
        {
            Item = item;
            ModifierFilters = modifierFilters;
        }

        /// <summary>
        /// The item for which to save modifiers for
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// The modifiers to save
        /// </summary>
        public ModifierFilters ModifierFilters { get; }
    }
}
