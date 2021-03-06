using System;

namespace Sidekick.Domain.Platforms
{
    /// <summary>
    /// Service providing keyboard functions
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
        event Action<string> OnKeyDown;

        /// <summary>
        /// Indicates if the input includes a modifer (Ctrl, Alt, Shift) key pressed.
        /// </summary>
        /// <returns>If a modifier is in the input, returns true.</returns>
        bool IncludesModifier(string input);

        /// <summary>
        /// Command to send keystrokes to the system
        /// </summary>
        /// <param name="keys">The keys to send</param>
        void PressKey(params string[] keys);

        /// <summary>
        /// Converts the system keybind to the Electron equivalent
        /// </summary>
        /// <param name="key">The key to convert</param>
        /// <returns>The electron equivalent key</returns>
        string ToElectronAccelerator(string key);
    }
}
