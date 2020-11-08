using MediatR;

namespace Sidekick.Domain.Game.Shortcuts
{
    /// <summary>
    /// Triggers a Find item action inside Path of Exile with the name of the item under the cursor
    /// </summary>
    public class FindItemCommand : ICommand<bool>
    {
    }
}
