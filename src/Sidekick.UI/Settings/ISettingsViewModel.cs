using System.ComponentModel;
using Sidekick.Core.Settings;
using Sidekick.UI.Helpers;

namespace Sidekick.UI.Settings
{
    public interface ISettingsViewModel : INotifyPropertyChanged
    {
        ObservableDictionary<string, string> Keybinds { get; }
        string CurrentKey { get; set; }
        SidekickSettings Settings { get; }
        ObservableDictionary<string, string> WikiOptions { get; }
        ObservableDictionary<string, string> UILanguageOptions { get; }

        bool IsKeybindUsed(string keybind, string ignoreKey = null);
        void Save();
    }
}
