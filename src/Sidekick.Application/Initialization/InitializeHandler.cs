using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Natives;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Application.Initialization
{
    public class InitializeHandler : IRequestHandler<InitializeCommand>
    {
        private readonly ILogger logger;
        private readonly IStringLocalizer<InitializeHandler> localizer;
        private readonly IMediator mediator;
        private readonly ServiceFactory serviceFactory;
        private readonly INativeNotifications nativeNotifications;
        private readonly INativeApp nativeApp;

        public InitializeHandler(
            ILogger<InitializeHandler> logger,
            IStringLocalizer<InitializeHandler> localizer,
            IMediator mediator,
            ServiceFactory serviceFactory,
            INativeNotifications nativeNotifications,
            INativeApp nativeApp)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.mediator = mediator;
            this.serviceFactory = serviceFactory;
            this.nativeNotifications = nativeNotifications;
            this.nativeApp = nativeApp;
        }

        public async Task<Unit> Handle(InitializeCommand request, CancellationToken cancellationToken)
        {
            await mediator.Publish(new InitializationStarted());

            var steps = new List<IInitializerStep>()
            {
                new InitializerStep<UpdateInitializationStarted>(mediator, serviceFactory, "Update", runOnce: true),
                new InitializerStep<SettingsInitializationStarted>(mediator, serviceFactory, "Settings"),
                new InitializerStep<LanguageInitializationStarted>(mediator, serviceFactory, "Language"),
                new InitializerStep<DataInitializationStarted>(mediator, serviceFactory, "Data"),
                new InitializerStep<KeybindsInitializationStarted>(mediator, serviceFactory, "Keybinds", runOnce: true),
            };

            foreach (var step in steps)
            {
                await ReportProgress(steps, step);
                try
                {
                    await step.Run(request.FirstRun);
                }
                catch (Exception exception)
                {
                    logger.LogError($"Initializer Error - {step.Name} - {exception.Message}", exception);
                    nativeNotifications.ShowMessage(localizer["Error"]);
                    nativeApp.Shutdown();
                    return Unit.Value;
                }
                await ReportProgress(steps, step);
            }

            await mediator.Publish(new InitializationCompleted());

            return Unit.Value;
        }

        private async Task ReportProgress(List<IInitializerStep> steps, IInitializerStep step)
        {
            var count = steps.Sum(x => x.Count);
            var completed = steps.Sum(x => x.Completed);
            var percentage = count == 0 ? 0 : (completed) * 100 / (count);

            var args = new InitializationProgressed()
            {
                Title = localizer["Title", steps.IndexOf(step) + 1, steps.Count],
                TotalPercentage = percentage,
            };

            if (percentage >= 100)
            {
                args.Title = localizer["Ready"];
            }

            await mediator.Publish(args);
        }

    }
}
