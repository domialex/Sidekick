using System;
using System.Text.Json;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Notifications.Commands;
using Sidekick.Localization.Update;

namespace Sidekick.Presentation.Blazor.Electron.Update
{
    public class UpdateProvider
    {
        private readonly IMediator mediator;
        private readonly ILogger<UpdateProvider> logger;
        private readonly UpdateResources resources;

        public UpdateProvider(
            IMediator mediator,
            ILogger<UpdateProvider> logger,
            UpdateResources resources)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.resources = resources;
        }

        public async Task Initialize()
        {
            // Auto Update
            try
            {
                ElectronNET.API.Electron.AutoUpdater.AutoDownload = true;
                ElectronNET.API.Electron.AutoUpdater.AutoInstallOnAppQuit = true;
                ElectronNET.API.Electron.AutoUpdater.AllowPrerelease = true;
                ElectronNET.API.Electron.AutoUpdater.OnUpdateAvailable += (info) =>
                {
                    logger.LogInformation("Update available! " + JsonSerializer.Serialize(info));
                    _ = mediator.Send(new OpenNotificationCommand("Version " + info.Version + " is being downloaded now. Once the download is complete. You can restart the application to get the update.", "Update Available!"));
                };
                ElectronNET.API.Electron.AutoUpdater.OnUpdateDownloaded += (info) =>
                {
                    logger.LogInformation("Update downloaded! " + JsonSerializer.Serialize(info));
                    _ = mediator.Send(new OpenNotificationCommand("Version " + info.Version + " is ready to use. You can restart the application to use the update.", "Update Ready!"));
                };
                await ElectronNET.API.Electron.AutoUpdater.CheckForUpdatesAndNotifyAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Could not update Sidekick.");
            }
        }
    }
}
