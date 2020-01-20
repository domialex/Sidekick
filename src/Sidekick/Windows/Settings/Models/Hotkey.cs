using System.Text;
using WindowsHook;

namespace Sidekick.Windows.Settings.Models
{
    public class Hotkey
    {
        public Keys Key { get; }

        public Keys Modifiers { get; }

        public Hotkey(Keys key, Keys modifiers)
        {
            Key = key;
            Modifiers = modifiers;
        }

        public override string ToString()
        {
            var str = new StringBuilder();

            if (Modifiers.HasFlag(Keys.Control))
                str.Append("Ctrl + ");
            if (Modifiers.HasFlag(Keys.Shift))
                str.Append("Shift + ");
            if (Modifiers.HasFlag(Keys.Alt))
                str.Append("Alt + ");
            if (Modifiers.HasFlag(Keys.LWin))
                str.Append("Win + ");

            str.Append(Key);

            return str.ToString();
        }
    }
}
