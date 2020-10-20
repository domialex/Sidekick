using System;
using System.ComponentModel;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Localization.Initialize;
using Sidekick.Localization.Tray;

namespace Sidekick.Initialization
{
    public class InitializationViewModel : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly IMediator mediator;
        private readonly INativeNotifications nativeNotifications;
        private readonly INativeApp nativeApp;
        private readonly SidekickSettings settings;

        public InitializationViewModel(IMediator mediator,
            INativeNotifications nativeNotifications,
            INativeApp nativeApp,
            SidekickSettings settings)
        {
            this.mediator = mediator;
            this.nativeNotifications = nativeNotifications;
            this.nativeApp = nativeApp;
            this.settings = settings;
        }

        public async Task Initialize()
        {
            await mediator.Send(new InitializeCommand(true)
            {
                OnProgress = (args) =>
                {
                    Title = args.Title;
                    Percentage = args.TotalPercentage;
                    StepPercentage = args.StepPercentage;
                    StepTitle = args.StepTitle;

                    if (Percentage >= 100)
                    {
                        Task.Run(async () =>
                        {
                            await Task.Delay(400);
                            if (Initialized != null)
                            {
                                Initialized.Invoke();
                            }
                        });
                    }
                },
                OnError = () =>
                {
                    nativeNotifications.ShowMessage(InitializeResources.ErrorDuringInit);
                    nativeApp.Shutdown();
                }
            });

            nativeNotifications.ShowSystemNotification(
                TrayResources.Notification_Title,
                string.Format(TrayResources.Notification_Message, settings.Key_CheckPrices.ToKeybindString(), settings.Key_CloseWindow.ToKeybindString())
            );
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
    }
}
