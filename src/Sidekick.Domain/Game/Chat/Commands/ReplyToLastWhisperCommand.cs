using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Command to start replying to the last whisper message inside Path of Exile
    /// </summary>
    public class ReplyToLastWhisperCommand : ICommand<bool>
    {
    }
}
