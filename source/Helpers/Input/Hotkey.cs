using System.Windows.Forms;
using Sidekick.Helpers.NativeMethods;
using Sidekick.Windows.Overlay;

namespace Sidekick.Helpers
{
    /// <summary>
    /// Bitwise enum containing hotkey instructions.
    /// </summary>
    public enum HotKeyRestrictions
    {
        None = 0,
        PathOfExileFocused = 1,
        OverlayDisplayed = 2,
        OverlayClosed = 4
    }

    /// <summary>
    /// Hotkey class used for softcoding hotkeys.
    /// </summary>
    public class Hotkey
    {
        /// <summary>
        /// Constructor for the hotkey class
        /// </summary>
        /// <param name="modifier">The key modifier. e.g (alt, control, shift)</param>
        /// <param name="key">The key to be pressed</param>
        /// <param name="handleEvent">Whether to handle the KeyEventArgs event</param>
        /// <param name="restrictions">Restrictions for when the hotkey action will trigger</param>
        public Hotkey(Keys modifier, Keys key, bool handleEvent, HotKeyRestrictions restrictions)
        {
            Modifier = modifier;
            Key = key;
            HandleEvent = handleEvent;
            Restrictions = restrictions;
        }

        /// <summary>
        /// The key modifier e.g (alt, control, shift)
        /// </summary>
        public Keys Modifier { get; set; }

        /// <summary>
        /// The key to be pressed
        /// </summary>
        public Keys Key { get; set; }

        /// <summary>
        /// Whether to handle the KeyEventArgs event
        /// </summary>
        public bool HandleEvent { get; set; }

        /// <summary>
        /// Restrictions for when the hotkey will trigger
        /// </summary>
        public HotKeyRestrictions Restrictions { get; set; }

        /// <summary>
        /// Determines if the key has been pressed based on the KeyEventArgs arguments
        /// </summary>
        /// <param name="e">KeyEventArgs arguments</param>
        /// <returns>True if pressed</returns>
        public bool IsPressed(KeyEventArgs e)
        {
            return e.Modifiers == Modifier && e.KeyCode == Key;
        }

        /// <summary>
        /// Determines if the restrictions have been met
        /// </summary>
        /// <returns>True if met</returns>
        public bool MeetsRestrictions()
        {
            var meets = true;

            if (meets && Restrictions.HasFlag(HotKeyRestrictions.PathOfExileFocused) && !ProcessHelper.IsPathOfExileInFocus())
                meets = false;

            if (meets && Restrictions.HasFlag(HotKeyRestrictions.OverlayDisplayed) && !OverlayController.IsDisplayed)
                meets = false;

            if (meets && Restrictions.HasFlag(HotKeyRestrictions.OverlayClosed) && OverlayController.IsDisplayed)
                meets = false;

            return meets;
        }

        public override bool Equals(object obj)
        {
            if (obj is Hotkey hotkey)
            {
                return hotkey.Key == Key && hotkey.Modifier == Modifier;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return Modifier.GetHashCode() + Key.GetHashCode();
        }
    }
}
