using System.Threading.Tasks;

namespace Sidekick.Domain.Keybinds
{
    /// <summary>
    /// Interface for chat keybinds to send a message in the chat
    /// </summary>
    public interface IChatKeybindHandler : IBaseKeybindHandler
    {
        /// <summary>
        /// Send a message in the chat. Wildcards are supported.
        /// {Me.CharacterName} for your own character name
        /// {LastWhisper.CharacterName} for the character name of the last whisper your received
        /// </summary>
        /// <param name="command">The message to write in the chat</param>
        /// <param name="submit">Indicated if the message should be submitted after it is written</param>
        Task Execute(string command, bool submit);
    }
}
