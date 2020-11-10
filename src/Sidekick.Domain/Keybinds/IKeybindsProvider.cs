using System;

namespace Sidekick.Domain.Keybinds
{
    /// <summary>
    /// Service providing keybind functions
    /// </summary>
    public interface IKeybindsProvider
    {
        /// <summary>
        /// Initialize the provider
        /// </summary>
        void Initialize();

        /// <summary>
        /// Event that indicates that a key was pressed
        /// </summary>
        event Func<string, bool> OnKeyDown;

        /// <summary>
        /// Event that indicates a scroll down input occured
        /// </summary>
        event Func<bool> OnScrollDown;

        /// <summary>
        /// Event that indicates a scroll up input occured
        /// </summary>
        event Func<bool> OnScrollUp;

        /// <summary>
        /// Command to send keystrokes to the system
        /// </summary>
        /// <param name="keys">The keys to send</param>
        void PressKey(string keys);
    }
}
