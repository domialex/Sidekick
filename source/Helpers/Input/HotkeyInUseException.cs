using System;

namespace Sidekick.Helpers
{
    /// <summary>
    /// Exception class used for when a hotkey is already in use.
    /// </summary>
    public class HotkeyInUseException : Exception
    {
        public HotkeyInUseException(Hotkey hotkey)
        {
            Hotkey = hotkey;
        }

        public Hotkey Hotkey { get; }
    }
}
