using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Start writing a message inside the game chat
    /// </summary>
    public class StartWritingChatCommand : ICommand
    {
        /// <summary>
        /// Start writing a message inside the game chat
        /// </summary>
        /// <param name="message">The message to start writing in the chat</param>
        public StartWritingChatCommand(string message)
        {
            Message = message;
        }

        /// <summary>
        /// The message to start writing in the chat
        /// </summary>
        public string Message { get; }
    }
}
