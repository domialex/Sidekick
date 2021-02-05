using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Settings;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Game.Leagues.Queries;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Initialization.Queries;
using Sidekick.Domain.Notifications.Commands;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Initialization
{
    public class InitializeHandler : ICommandHandler<InitializeCommand>
    {
        private readonly IStringLocalizer<InitializeHandler> localizer;
        private readonly IMediator mediator;
        private readonly ServiceFactory serviceFactory;
        private readonly ISidekickSettings settings;
        private readonly ILogger<InitializeHandler> logger;
        private readonly IViewLocator viewLocator;

        public InitializeHandler(
            IStringLocalizer<InitializeHandler> localizer,
            IMediator mediator,
            ServiceFactory serviceFactory,
            ISidekickSettings settings,
            ILogger<InitializeHandler> logger,
            IViewLocator viewLocator)
        {
            this.localizer = localizer;
            this.mediator = mediator;
            this.serviceFactory = serviceFactory;
            this.settings = settings;
            this.logger = logger;
            this.viewLocator = viewLocator;
        }

        private int Count = 0;

        private int Completed = 0;

        private void AddCount<TNotification>(bool shouldAdd = true)
            where TNotification : INotification
        {
            if (!shouldAdd) return;

            Count += serviceFactory.GetInstances<INotificationHandler<TNotification>>().Count();
        }

        public async Task<Unit> Handle(InitializeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Completed = 0;
                Count = 0;

                // Set the total count of handlers
                AddCount<PlatformInitializationStarted>(request.FirstRun);
                AddCount<LanguageInitializationStarted>();
                AddCount<DataInitializationStarted>();

                // Report initial progress
                await ReportProgress();

                // Open a clean view of the initialization
                viewLocator.Close(View.Settings);
                viewLocator.Close(View.Setup);
                if (settings.ShowSplashScreen && !viewLocator.IsOpened(View.Initialization))
                {
                    await viewLocator.Open(View.Initialization);
                }

                await RunStep<LanguageInitializationStarted>();

                // Check for updates
                if (await mediator.Send(new IsNewVersionAvailableQuery()))
                {
                    await mediator.Send(new OpenConfirmNotificationCommand(localizer["UpdateAvailable"], localizer["UpdateTitle"], async () =>
                    {
                        await mediator.Send(new OpenBrowserCommand(new Uri("https://github.com/domialex/Sidekick/releases")));
                        await mediator.Send(new ShutdownCommand());
                    }));
                }

                // Check to see if we should run Setup first before running the rest of the initialization process
                if (string.IsNullOrEmpty(settings.LeagueId) || string.IsNullOrEmpty(settings.Language_Parser) || string.IsNullOrEmpty(settings.Language_UI))
                {
                    await mediator.Send(new SetupCommand());
                    return Unit.Value;
                }

                if (request.FirstRun)
                {
                    var leagues = await mediator.Send(new GetLeaguesQuery(false));
                    var leaguesHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(leagues)));

                    if (leaguesHash != settings.LeaguesHash)
                    {
                        await mediator.Send(new ClearCacheCommand());
                        await mediator.Send(new SaveSettingCommand(nameof(ISidekickSettings.LeaguesHash), leaguesHash));
                    }

                    // Check to see if we should run Setup first before running the rest of the initialization process
                    if (string.IsNullOrEmpty(settings.LeagueId) || !leagues.Any(x => x.Id == settings.LeagueId))
                    {
                        await mediator.Send(new OpenNotificationCommand(localizer["NewLeagues"]));
                        await mediator.Send(new SetupCommand());
                        return Unit.Value;
                    }
                }

                await RunStep<DataInitializationStarted>();
                await RunStep<PlatformInitializationStarted>(request.FirstRun);

                // If we have a successful initialization, we delay for half a second to show the "Ready" label on the UI before closing the view
                await ReportProgress();
                await Task.Delay(500);

                // Show a system notification
                await mediator.Send(new OpenNotificationCommand(string.Format(localizer["Notification_Message"], settings.Price_Key_Check.ToKeybindString(), settings.Price_Key_Close.ToKeybindString()), true)
                {
                    Title = localizer["Notification_Title"],
                });

                viewLocator.Close(View.Initialization);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                await mediator.Send(new OpenNotificationCommand(localizer["Error"]));
                await mediator.Send(new ShutdownCommand());
                return Unit.Value;
            }
        }

        private async Task RunStep<TNotification>(bool shouldRun = true)
            where TNotification : INotification, new()
        {
            if (!shouldRun) return;

            // Publish the notification
            await mediator.Publish(new TNotification());

            // Make sure that after all handlers run, the Completed count is updated
            Completed += serviceFactory.GetInstances<INotificationHandler<TNotification>>().Count();

            // Report progress
            await ReportProgress();
        }

        private async Task ReportProgress()
        {
            var args = new InitializationProgressed(Count == 0 ? 0 : (Completed) * 100 / (Count));
            if (args.Percentage >= 100)
            {
                args.Title = localizer["Ready"];
                args.Percentage = 100;
                logger.LogError($"PROGRESS {args.Percentage} {args.Title}");
            }
            else
            {
                args.Title = localizer["Title", Completed, Count];
            }

            await mediator.Publish(args);
        }
    }
}
