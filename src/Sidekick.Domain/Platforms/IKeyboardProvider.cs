using System;
using System.Threading.Tasks;

namespace Sidekick.Domain.Platforms
{
    /// <summary>
    /// Service providing keybind functions
    /// </summary>
    public interface IKeyboardProvider
    {
        /// <summary>
        /// Initialize the provider
        /// </summary>
        void Initialize();

        /// <summary>
        /// Event that indicates that a key was pressed
        /// </summary>
        event Func<KeyDownArgs, bool> OnKeyDown;

        /// <summary>
        /// Gets the state of the Ctrl key.
        /// </summary>
        /// <returns>If it is pressed, returns true.</returns>
        bool IsCtrlPressed();

        /// <summary>
        /// Command to send keystrokes to the system
        /// </summary>
        /// <param name="keys">The keys to send</param>
        Task PressKey(params string[] keys);
    }
}
