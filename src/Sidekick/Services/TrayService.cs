using System.Threading.Tasks;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;

namespace Sidekick.Services
{
    public class TrayService : ITrayService, IOnAfterInit
    {
        private readonly Configuration configuration;

        public TrayService(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public Task OnAfterInit()
        {
            App.ShowNotifcation(
                "Sidekick is ready",
                $"Press {configuration.KeyPriceCheck.ToKeybindString()} over an item in-game to use. Press {configuration.KeyCloseWindow.ToKeybindString()} to close overlay.");

            return Task.CompletedTask;
        }
    }
}
