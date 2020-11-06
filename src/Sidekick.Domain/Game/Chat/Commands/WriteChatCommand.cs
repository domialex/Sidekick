using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Writes a message inside the game chat
    /// </summary>
    public class WriteChatCommand : ICommand
    {
        /// <summary>
        /// Writes a message inside the game chat
        /// </summary>
        /// <param name="message">The message to write in the chat</param>
        public WriteChatCommand(string message)
        {
            Message = message;
        }

        /// <summary>
        /// The message to write in the chat
        /// </summary>
        public string Message { get; }
    }
}
