using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Core.Update;
using Sidekick.Localization.Initialize;
using Sidekick.Localization.Tray;

namespace Sidekick.Views.Initialize
{
    public class InitializeViewModel : IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly IInitializer initializer;
        private readonly IUpdater updater;
        private readonly INativeNotifications nativeNotifications;
        private readonly INativeApp nativeApp;
        private readonly SidekickSettings settings;
        private bool isDisposed;

        public InitializeViewModel(IInitializer initializer,
            IUpdater updater,
            INativeNotifications nativeNotifications,
            INativeApp nativeApp,
            SidekickSettings settings)
        {
            this.initializer = initializer;
            this.updater = updater;
            this.nativeNotifications = nativeNotifications;
            this.nativeApp = nativeApp;
            this.settings = settings;

            initializer.OnProgress += Initializer_OnProgress;
            initializer.OnError += Initializer_OnError;
        }

        public async Task Initialize()
        {
            await updater.Update();
            await initializer.Initialize();

            nativeNotifications.ShowSystemNotification(
                TrayResources.Notification_Title,
                string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString())
            );
        }

        private void Initializer_OnError(ErrorEventArgs obj)
        {
            nativeNotifications.ShowMessage(InitializeResources.ErrorDuringInit);
            nativeApp.Shutdown();
        }

        public void Close()
        {
            nativeApp.Shutdown();
        }

        public string Title { get; set; }
        public int Percentage { get; set; }

        public string StepTitle { get; set; }
        public int StepPercentage { get; set; }

        public double ProgressValue => Percentage / 100.0;

        public event Action Initialized;

        private void Initializer_OnProgress(ProgressEventArgs args)
        {
            Title = args.Title;
            Percentage = args.TotalPercentage;
            StepPercentage = args.StepPercentage;
            StepTitle = args.StepTitle;

            if (Percentage == 100)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(800);
                    if (Initialized != null)
                    {
                        Initialized.Invoke();
                    }
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
            }

            isDisposed = true;
        }
    }
}
