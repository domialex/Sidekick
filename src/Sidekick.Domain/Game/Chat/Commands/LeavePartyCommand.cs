using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Command to leave the current party inside Path of Exile
    /// </summary>
    public class LeavePartyCommand : ICommand<bool>
    {
    }
}
