using System;
using System.Threading.Tasks;
using PropertyChanged;
using Sidekick.Core.Initialization;
using Sidekick.Localization.Splash;

namespace Sidekick.UI.Splash
{
    [AddINotifyPropertyChangedInterface]
    public class SplashViewModel : IDisposable, ISplashViewModel
    {
        private readonly IInitializer initializer;

        public SplashViewModel(IInitializer initializer)
        {
            this.initializer = initializer;

            initializer.OnProgress += Initializer_OnProgress;
        }

        public int StepPercentage { get; set; }
        public string StepTitle { get; set; }

        public int Percentage { get; set; }
        public string Title { get; set; }

        public event Action Initialized;

        private void Initializer_OnProgress(ProgressEventArgs args)
        {
            StepPercentage = args.Percentage;
            StepTitle = args.ServiceName;
            Percentage = args.TotalPercentage;

            Title = SplashResources.ResourceManager.GetString($"Type_{args.Type.ToString()}");
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
            initializer.OnProgress -= Initializer_OnProgress;
        }
    }
}
