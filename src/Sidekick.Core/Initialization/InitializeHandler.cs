using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Initialization.Notifications;

namespace Sidekick.Core.Initialization
{
    public class InitializeHandler : IRequestHandler<InitializeCommand>
    {
        private readonly ILogger logger;
        private readonly IStringLocalizer<InitializeHandler> localizer;
        private readonly IMediator mediator;
        private readonly ServiceFactory serviceFactory;

        public InitializeHandler(
            ILogger<InitializeHandler> logger,
            IStringLocalizer<InitializeHandler> localizer,
            IMediator mediator,
            ServiceFactory serviceFactory)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.mediator = mediator;
            this.serviceFactory = serviceFactory;
        }

        public async Task<Unit> Handle(InitializeCommand request, CancellationToken cancellationToken)
        {
            var steps = new List<IInitializerStep>()
            {
                new InitializerStep<InitializeUpdateNotification>(mediator, serviceFactory, "Update", runOnce: true),
                new InitializerStep<InitializeSettingsNotification>(mediator, serviceFactory, "Settings"),
                new InitializerStep<InitializeLanguageNotification>(mediator, serviceFactory, "Language"),
                new InitializerStep<InitializeDataNotification>(mediator, serviceFactory, "Data"),
                new InitializerStep<InitializeKeybindsNotification>(mediator, serviceFactory, "Keybinds", runOnce: true),
            };

            foreach (var step in steps)
            {
                ReportProgress(request, steps, step, null, "Start Step");
                try
                {
                    await step.Run(
                        request.FirstRun,
                        (name) => ReportProgress(request, steps, step, name, "Start Service"),
                        (name) => ReportProgress(request, steps, step, name, "End Service")
                    );
                }
                catch (Exception exception)
                {
                    logger.LogError($"Initializer Error - {step.Name} - {exception.Message}", exception);
                    request.OnError?.Invoke();
                    return Unit.Value;
                }
                ReportProgress(request, steps, step, null, "End Step");
            }

            return Unit.Value;
        }

        private void ReportProgress(InitializeCommand request, List<IInitializerStep> steps, IInitializerStep step, string serviceName, string message)
        {
            var count = steps.Sum(x => x.Count);
            var completed = steps.Sum(x => x.Completed);
            var percentage = count == 0 ? 0 : (completed) * 100 / (count);

            var args = new ProgressNotification()
            {
                Title = localizer["Title", steps.IndexOf(step) + 1, steps.Count],
                TotalPercentage = percentage,
                StepTitle = serviceName,
                StepPercentage = step.Percentage,
            };

            if (percentage >= 100)
            {
                args.Title = localizer["Ready"];
                args.StepPercentage = 100;
            }

            request.OnProgress?.Invoke(args);

            logger.LogDebug($"Initializer - {percentage}% - {step.Name} - {message} {serviceName}");
        }

    }
}
