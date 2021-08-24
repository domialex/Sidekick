using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sidekick.Apis.GitHub;
using Sidekick.Common;
using Sidekick.Common.Blazor.Views;
using Sidekick.Modules.Update.Localization;

namespace Sidekick.Modules.Update.Pages
{
    public partial class Update : ComponentBase
    {
        [Inject] private ILogger<Update> Logger { get; set; }
        [Inject] private IViewLocator ViewLocator { get; set; }
        [Inject] private IGitHubClient GitHubClient { get; set; }
        [Inject] private IAppService AppService { get; set; }
        [Inject] private UpdateResources UpdateResources { get; set; }
        [Inject] private IViewInstance ViewInstance { get; set; }

        private int Count { get; set; } = 0;
        private int Completed { get; set; } = 0;
        private string Title { get; set; }
        private double Percentage => (double)Completed / Count;

        public static bool HasRun { get; set; } = false;

        protected override void OnInitialized()
        {
            Task.Run(Handle);
            base.OnInitialized();
        }

        public async Task Handle()
        {
            try
            {
                // Checking release
                Completed = 1;
                Count = 3;
                Title = UpdateResources.Checking;
                StateHasChanged();
                var release = await GitHubClient.GetLatestRelease();

                if (!GitHubClient.IsUpdateAvailable(release))
                {
                    Completed = 3;
                    StateHasChanged();
                    await ViewLocator.Open(View.Initialization);
                    await ViewInstance.Close();
                    return;
                }

                // Downloading
                Completed = 2;
                Title = UpdateResources.Downloading(release.Tag);
                StateHasChanged();
                var path = await GitHubClient.DownloadRelease(release);
                if (path == null)
                {
                    Completed = 3;
                    Title = UpdateResources.Failed;
                    StateHasChanged();
                    await Task.Delay(3000);
                    return;
                }

                // Downloaded
                await Task.Delay(1000);
                Process.Start(path);
                Exit();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                await AppService.OpenNotification(UpdateResources.Failed);
                AppService.Shutdown();
            }
        }

        public void Exit()
        {
            AppService.Shutdown();
        }
    }
}
