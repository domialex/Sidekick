using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;
using Sidekick.Windows.Settings;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Sidekick.Services
{
    public class TrayService : ITrayService, IOnAfterInit
    {
        public TrayService()
        {
        }

        public Task OnAfterInit()
        {
            App.ShowNotifcation(
                GetParsedString("Sidekick is ready"),
                GetParsedString($"Press {KeybindSetting.PriceCheck.GetTemplate()} over an item in-game to use. Press {KeybindSetting.CloseWindow.GetTemplate()} to close overlay."));

            return Task.CompletedTask;
        }

        private static string GetParsedString(string source)
        {
            var settings = SettingsController.GetSettingsInstance();

            foreach (var value in Enum.GetValues(typeof(GeneralSetting)).Cast<GeneralSetting>().ToList())
            {
                if (settings.GeneralSettings.ContainsKey(value))
                {
                    source = source.Replace(value.GetTemplate(), settings.GeneralSettings[value].ToString());
                }
            }

            foreach (var value in Enum.GetValues(typeof(KeybindSetting)).Cast<KeybindSetting>().ToList())
            {
                if (settings.KeybindSettings.ContainsKey(value))
                {
                    source = source.Replace(value.GetTemplate(), settings.KeybindSettings[value].ToString());
                }
            }

            return source;
        }
    }
}
