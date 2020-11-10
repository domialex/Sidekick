using MediatR;

namespace Sidekick.Domain.Game.Stashes.Commands
{
    /// <summary>
    /// Scroll the selected stash tab up inside the Path of Exile game
    /// </summary>
    public class ScrollStashUpCommand : ICommand<bool>
    {
    }
}
