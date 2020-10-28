using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Initialization.Queries;
using Sidekick.Domain.Leagues;

namespace Sidekick.Application.Initialization
{
    public class InitializeHandler : ICommandHandler<InitializeCommand>
    {
        private readonly ILogger logger;
        private readonly IStringLocalizer<InitializeHandler> localizer;
        private readonly IMediator mediator;
        private readonly ServiceFactory serviceFactory;
        private readonly INativeNotifications nativeNotifications;
        private readonly SidekickSettings settings;

        public InitializeHandler(
            ILogger<InitializeHandler> logger,
            IStringLocalizer<InitializeHandler> localizer,
            IMediator mediator,
            ServiceFactory serviceFactory,
            INativeNotifications nativeNotifications,
            SidekickSettings settings)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.mediator = mediator;
            this.serviceFactory = serviceFactory;
            this.nativeNotifications = nativeNotifications;
            this.settings = settings;
        }

        public async Task<Unit> Handle(InitializeCommand request, CancellationToken cancellationToken)
        {
            var steps = new List<IInitializerStep>()
            {
                new InitializerStep<LanguageInitializationStarted>(mediator, serviceFactory, "Language"),
                new InitializerStep<DataInitializationStarted>(mediator, serviceFactory, "Data"),
                new InitializerStep<KeybindsInitializationStarted>(mediator, serviceFactory, "Keybinds", runOnce: true),
            };

            // Report initial progress
            await ReportProgress(steps);

            // Let everyone know that the initialization process has started
            await mediator.Publish(new InitializationStarted());

            // Check for updates
            if (await mediator.Send(new IsNewVersionAvailableQuery()))
            {
                nativeNotifications.ShowYesNo(
                    localizer["UpdateAvailable"],
                    localizer["UpdateTitle"],
                    onYes: async () =>
                    {
                        await mediator.Send(new OpenBrowserCommand(new Uri("https://github.com/domialex/Sidekick/releases")));
                        await mediator.Send(new ShutdownCommand());
                    });
            }

            var runSetup = !settings.HasSetupCompleted;
            if (request.FirstRun)
            {
                var leagues = await mediator.Send(new GetLeaguesQuery(false));

                var leaguesHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(leagues)));
                if (leaguesHash != settings.LeaguesHash)
                {
                    await mediator.Send(new ClearCacheCommand());
                    settings.LeaguesHash = leaguesHash;
                    settings.Save();
                    if (!runSetup)
                    {
                        runSetup = true;
                        nativeNotifications.ShowMessage(localizer["NewLeagues"]);
                    }
                }

                runSetup = runSetup || string.IsNullOrEmpty(settings.LeagueId) || !leagues.Any(x => x.Id == settings.LeagueId);
            }

            // Check to see if we should run Setup first before running the rest of the initialization process
            if (runSetup)
            {
                await mediator.Send(new SetupCommand());
                return Unit.Value;
            }

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
            else
            {
                args.Title = localizer["Title", 0, steps.Count];
            }

            await mediator.Publish(args);
        }

    }
}
