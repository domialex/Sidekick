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
using Sidekick.Business.Caches;
using Sidekick.Business.Leagues;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Leagues;
using Sidekick.Domain.Natives.Initialization.Commands;

namespace Sidekick.Application.Initialization
{
    public class InitializeHandler : ICommandHandler<InitializeCommand>
    {
        private readonly ILogger logger;
        private readonly IStringLocalizer<InitializeHandler> localizer;
        private readonly IMediator mediator;
        private readonly ServiceFactory serviceFactory;
        private readonly INativeNotifications nativeNotifications;
        private readonly ILeagueDataService leagueDataService;
        private readonly ICacheService cacheService;
        private readonly SidekickSettings settings;

        public InitializeHandler(
            ILogger<InitializeHandler> logger,
            IStringLocalizer<InitializeHandler> localizer,
            IMediator mediator,
            ServiceFactory serviceFactory,
            INativeNotifications nativeNotifications,
            ILeagueDataService leagueDataService,
            ICacheService cacheService,
            SidekickSettings settings)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.mediator = mediator;
            this.serviceFactory = serviceFactory;
            this.nativeNotifications = nativeNotifications;
            this.leagueDataService = leagueDataService;
            this.cacheService = cacheService;
            this.settings = settings;
        }

        public async Task<Unit> Handle(InitializeCommand request, CancellationToken cancellationToken)
        {
            var steps = new List<IInitializerStep>()
            {
                new InitializerStep<UpdateInitializationStarted>(mediator, serviceFactory, "Update", runOnce: true),
                new InitializerStep<LanguageInitializationStarted>(mediator, serviceFactory, "Language"),
                new InitializerStep<DataInitializationStarted>(mediator, serviceFactory, "Data"),
                new InitializerStep<KeybindsInitializationStarted>(mediator, serviceFactory, "Keybinds", runOnce: true),
            };

            // Report initial progress
            await ReportProgress(steps);

            // Let everyone know that the initialization process has started
            await mediator.Publish(new InitializationStarted());

            var runSetup = !settings.HasSetupCompleted;
            if (request.FirstRun)
            {
                var leagues = await mediator.Send(new GetLeaguesQuery());
                leagueDataService.Initialize(leagues);

                var leaguesHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(leagues)));
                if (leaguesHash != settings.LeaguesHash)
                {
                    await cacheService.Clear();
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
