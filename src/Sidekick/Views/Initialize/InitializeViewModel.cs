using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Trade.Leagues;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Core.Update;
using Sidekick.Localization.Initializer;
using Sidekick.Localization.Splash;
using Sidekick.Localization.Tray;
using Sidekick.Localization.Update;
using Sidekick.Notifications;

namespace Sidekick.Views.Initialize
{
    public class InitializeViewModel : IDisposable, INotifyPropertyChanged
    {
        private readonly IInitializer initializer;
        private readonly IUpdateManager updateManager;
        private readonly INativeBrowser nativeBrowser;
        private readonly ILeagueDataService leagueDataService;
        private readonly INotificationManager notificationManager;
        private readonly App app;
        private readonly SidekickSettings settings;
        private bool isDisposed;

        public InitializeViewModel(IInitializer initializer,
            IUpdateManager updateManager,
            INativeBrowser nativeBrowser,
            ILeagueDataService leagueDataService,
            INotificationManager notificationManager,
            App app,
            SidekickSettings settings)
        {
            this.initializer = initializer;
            this.updateManager = updateManager;
            this.nativeBrowser = nativeBrowser;
            this.leagueDataService = leagueDataService;
            this.notificationManager = notificationManager;
            this.app = app;
            this.settings = settings;

            initializer.OnProgress += Initializer_OnProgress;
            initializer.OnError += Initializer_OnError;
            leagueDataService.OnNewLeagues += LeagueDataService_OnNewLeagues;
        }

        private void LeagueDataService_OnNewLeagues()
        {
            notificationManager.ShowMessage(InitializerResources.Warn_NewLeagues);
        }

        private void Initializer_OnError(ErrorEventArgs obj)
        {
            notificationManager.ShowMessage(InitializerResources.ErrorDuringInit);
            app.Shutdown(1);
        }

        public async Task Initialize()
        {
            await RunAutoUpdate();
            await initializer.Initialize();

            notificationManager.ShowSystemNotification(
                TrayResources.Notification_Title,
                string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString())
            );
        }

        public void Close()
        {
            app.Shutdown();
        }

        public int StepPercentage { get; set; }
        public string StepTitle { get; set; }

        public int Percentage { get; set; }
        public double ProgressValue => Percentage / 100.0;
        public string Title { get; set; }

        public event Action Initialized;
        public event PropertyChangedEventHandler PropertyChanged;

        private void Initializer_OnProgress(ProgressEventArgs args)
        {
            StepPercentage = args.Percentage;
            StepTitle = args.ServiceName;
            Percentage = args.TotalPercentage;

            Title = SplashResources.ResourceManager.GetString($"Type_{args.Type}");
            if (string.IsNullOrEmpty(Title))
            {
                Title = args.Type.ToString();
            }

            if (Percentage == 100 && Initialized != null)
            {
                StepTitle = string.Empty;
                StepPercentage = 100;
                Title = SplashResources.Ready;
                Task.Run(async () =>
                {
                    await Task.Delay(800);
                    Initialized?.Invoke();
                });
            }
        }

        private async Task RunAutoUpdate()
        {
            if (await updateManager.NewVersionAvailable())
            {
                notificationManager.ShowYesNo(
                    UpdateResources.UpdateAvailable,
                    UpdateResources.Title,
                    onYes: () =>
                    {
                        nativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                        app.Shutdown();

                        //try
                        //{
                        //    if (await updateManagerService.UpdateSidekick())
                        //    {
                        //        nativeProcess.Mutex = null;
                        //        AdonisUI.Controls.MessageBox.Show(UpdateResources.UpdateCompleted, UpdateResources.Title, AdonisUI.Controls.MessageBoxButton.OK);

                        //        var startInfo = new ProcessStartInfo
                        //        {
                        //            FileName = Path.Combine(updateManagerService.InstallDirectory, "Sidekick.exe"),
                        //            UseShellExecute = false,
                        //        };
                        //        Process.Start(startInfo);
                        //    }
                        //    else
                        //    {
                        //        AdonisUI.Controls.MessageBox.Show(UpdateResources.UpdateFailed, UpdateResources.Title);
                        //        nativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                        //    }

                        //    Current.Shutdown();
                        //}
                        //catch (Exception)
                        //{
                        //    MessageBox.Show(UpdateResources.UpdateFailed, UpdateResources.Title);
                        //    nativeBrowser.Open(new Uri("https://github.com/domialex/Sidekick/releases"));
                        //}
                    });
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                initializer.OnProgress -= Initializer_OnProgress;
                initializer.OnError -= Initializer_OnError;
                leagueDataService.OnNewLeagues -= LeagueDataService_OnNewLeagues;
            }

            isDisposed = true;
        }
    }
}
