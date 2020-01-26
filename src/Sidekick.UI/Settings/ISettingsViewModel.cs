using System.Collections.Generic;
using Sidekick.Core.Settings;

namespace Sidekick.UI.Settings
{
    public interface ISettingsViewModel
    {
        Dictionary<string, string> Keybinds { get; }
        SidekickSettings Settings { get; }

        bool IsKeybindUsed(string keybind, string ignoreKey = null);
        void Save();
    }
}