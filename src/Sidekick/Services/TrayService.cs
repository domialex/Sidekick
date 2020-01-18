using Sidekick.Business.Leagues;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;
using Sidekick.Helpers;
using System.Threading.Tasks;

namespace Sidekick.Services
{
    public class TrayService : ITrayService, IOnAfterInitialize
    {
        private readonly ILeagueService leagueService;

        public TrayService(ILeagueService leagueService)
        {
            this.leagueService = leagueService;
        }

        public Task OnAfterInitialize()
        {
            TrayIcon.ReloadUI();
            TrayIcon.PopulateLeagueSelectMenu(leagueService.Leagues);
            TrayIcon.SendNotification("Sidekick is ready", $"Press {KeybindSetting.PriceCheck.GetTemplate()} over an item in-game to use. Press {KeybindSetting.CloseWindow.GetTemplate()} to close overlay.");

            return Task.CompletedTask;
        }
    }
}
