using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Exit the Path of Exile game to the character selection screen
    /// </summary>
    public class ExitToCharacterSelectionCommand : ICommand<bool>
    {
    }
}
