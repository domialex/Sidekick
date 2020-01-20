using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;
using Sidekick.Helpers;
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
            TrayIcon.ReloadUI();
            TrayIcon.SendNotification($"Press {KeybindSetting.PriceCheck.GetTemplate()} over an item in-game to use. Press {KeybindSetting.CloseWindow.GetTemplate()} to close overlay.", "Sidekick is ready");

            return Task.CompletedTask;
        }
    }
}
