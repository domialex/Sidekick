using MediatR;

namespace Sidekick.Domain.Game.Chat.Commands
{
    /// <summary>
    /// Send a message in the chat
    /// </summary>
    public class ChatCommand : ICommand<bool>
    {
        /// <summary>
        /// Send a message in the chat. Wildcards are supported.
        /// {Me.CharacterName} for your own character name
        /// {LastWhisper.CharacterName} for the character name of the last whisper your received
        /// </summary>
        /// <param name="command">The message to write in the chat</param>
        /// <param name="submit">Indicated if the message should be submitted after it is written</param>
        public ChatCommand(string command, bool submit)
        {
            Command = command;
            Submit = submit;
        }

        /// <summary>
        /// The message to write in the chat
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Indicated if the message should be submitted after it is written
        /// </summary>
        public bool Submit { get; }
    }
}
