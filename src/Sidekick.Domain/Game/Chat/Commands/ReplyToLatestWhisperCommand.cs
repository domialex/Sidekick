using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Start writing to the last person that has messaged the player inside Path of Exile
    /// </summary>
    public class ReplyToLatestWhisperCommand : ICommand<bool>
    {
    }
}
