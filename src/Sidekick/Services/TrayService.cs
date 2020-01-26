using System.Threading.Tasks;
using Sidekick.Core.Settings;
using Sidekick.Core.Initialization;

namespace Sidekick.Services
{
    public class TrayService : ITrayService, IOnAfterInit
    {
        private readonly SidekickSettings settings;

        public TrayService(SidekickSettings settings)
        {
            this.settings = settings;
        }

        public Task OnAfterInit()
        {
            App.ShowNotifcation(
                "Sidekick is ready",
                $"Press {settings.KeyPriceCheck.ToKeybindString()} over an item in-game to use. Press {settings.KeyCloseWindow.ToKeybindString()} to close overlay.");

            return Task.CompletedTask;
        }
    }
}
