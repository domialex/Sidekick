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
using Sidekick.Domain.Natives.Initialization.Commands;

namespace Sidekick.Application.Initialization
{
    public class InitializeHandler : IRequestHandler<InitializeCommand>
    {
        private readonly ILogger logger;
        private readonly IStringLocalizer<InitializeHandler> localizer;
        private readonly IMediator mediator;
        private readonly ServiceFactory serviceFactory;
        private readonly INativeNotifications nativeNotifications;

        public InitializeHandler(
            ILogger<InitializeHandler> logger,
            IStringLocalizer<InitializeHandler> localizer,
            IMediator mediator,
            ServiceFactory serviceFactory,
            INativeNotifications nativeNotifications)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.mediator = mediator;
            this.serviceFactory = serviceFactory;
            this.nativeNotifications = nativeNotifications;
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
                try
                {
                    await ReportProgress(steps, step);
                    await step.Run(request.FirstRun);
                }
                catch (Exception exception)
                {
                    logger.LogError($"Initializer Error - {step.Name} - {exception.Message}", exception);
                    nativeNotifications.ShowMessage(localizer["Error"]);
                    await mediator.Send(new ShutdownCommand());
                    return Unit.Value;
                }
            }

            await ReportProgress(steps);
            await mediator.Publish(new InitializationCompleted());

            return Unit.Value;
        }

        private async Task ReportProgress(List<IInitializerStep> steps, IInitializerStep step = null)
        {
            var count = steps.Sum(x => x.Count);
            var completed = steps.Sum(x => x.Completed);
            var percentage = count == 0 ? 0 : (completed) * 100 / (count);

            var args = new InitializationProgressed(percentage);

            if (step != null)
            {
                args.Title = localizer["Title", steps.IndexOf(step) + 1, steps.Count];
            }
            else if (percentage >= 100)
            {
                args.Title = localizer["Ready"];
            }

            await mediator.Publish(args);
        }

    }
}
