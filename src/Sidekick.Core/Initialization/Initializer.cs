using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Serilog;

namespace Sidekick.Core.Initialization
{
    public class Initializer : IInitializer
    {
        private readonly ILogger logger;
        private readonly IStringLocalizer<Initializer> localizer;

        public Initializer(
            ILogger logger,
            IStringLocalizer<Initializer> localizer,
            IMediator mediator,
            ServiceFactory serviceFactory)
        {
            this.logger = logger.ForContext(GetType());
            this.localizer = localizer;

            Steps = new List<IInitializerStep>()
            {
                new InitializerStep<InitializeUpdateNotification>(mediator, serviceFactory, "Update", runOnce: true),
                new InitializerStep<InitializeSettingsNotification>(mediator, serviceFactory, "Settings"),
                new InitializerStep<InitializeLanguageNotification>(mediator, serviceFactory, "Language"),
                new InitializerStep<InitializeDataNotification>(mediator, serviceFactory, "Data"),
                new InitializerStep<InitializeKeybindsNotification>(mediator, serviceFactory, "Keybinds", runOnce: true),
            };
        }

        public event Action OnError;

        private List<IInitializerStep> Steps { get; set; }

        #region Progress
        private int TotalCount => Steps.Sum(x => x.Count);
        private int TotalCompleted => Steps.Sum(x => x.Completed);
        private int TotalPercentage => TotalCount == 0 ? 0 : (TotalCompleted) * 100 / (TotalCount);

        public event Action<ProgressEventArgs> OnProgress;

        private void ReportProgress(IInitializerStep step, string serviceName, string message)
        {
            var args = new ProgressEventArgs()
            {
                Title = localizer["Title", Steps.IndexOf(step) + 1, Steps.Count],
                TotalPercentage = TotalPercentage,
                StepTitle = serviceName,
                StepPercentage = step.Percentage,
            };

            if (TotalPercentage >= 100)
            {
                args.Title = localizer["Ready"];
                args.StepPercentage = 100;
                serviceName = string.Empty;
            }

            OnProgress?.Invoke(args);

            logger.Debug($"Initializer - {TotalPercentage}% - {step.Name} - {message} - {serviceName}");
        }

        #endregion

        private bool IsInitializing { get; set; } = false;

        public async Task Initialize(bool firstRun)
        {
            if (IsInitializing) { return; }
            IsInitializing = true;

            Steps.ForEach(x => x.Completed = 0);

            foreach (var step in Steps)
            {
                try
                {
                    await step.Run(
                        firstRun,
                        (name) => ReportProgress(step, name, "Start"),
                        (name) => ReportProgress(step, name, "End")
                    );
                }
                catch (Exception exception)
                {
                    logger.Error($"Initializer Error - {step.Name} - {exception.Message}", exception);
                    OnError?.Invoke();
                    return;
                }
            }

            IsInitializing = false;

        }
    }
}
