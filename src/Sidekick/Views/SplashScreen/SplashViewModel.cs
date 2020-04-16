using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Localization.Splash;

namespace Sidekick.Views.SplashScreen
{
    public class SplashViewModel : IDisposable, INotifyPropertyChanged
    {
        private readonly IInitializer initializer;
        private bool isDisposed;

        public SplashViewModel(IInitializer initializer)
        {
            this.initializer = initializer;

            initializer.OnProgress += Initializer_OnProgress;
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
            }

            isDisposed = true;
        }
    }
}
