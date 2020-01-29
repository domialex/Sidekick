using System.Collections.Generic;
using System.ComponentModel;
using Sidekick.Core.Settings;

namespace Sidekick.UI.Settings
{
    public interface ISettingsViewModel : INotifyPropertyChanged
    {
        Dictionary<string, string> Keybinds { get; }
        KeyValuePair<string, string>? CurrentKeybind { get; set; }
        SidekickSettings Settings { get; }
        Dictionary<string, string> WikiOptions { get; }
        Dictionary<string, string> UILanguageOptions { get; }

        bool IsKeybindUsed(string keybind, string ignoreKey = null);
        void Save();
    }
}
