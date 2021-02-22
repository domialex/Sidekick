using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Settings;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Cache.Commands;
using Sidekick.Domain.Game.Languages.Commands;
using Sidekick.Domain.Game.Leagues.Queries;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Initialization.Queries;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Localization;
using Sidekick.Domain.Notifications.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views;
using Sidekick.Localization.Initialization;

namespace Sidekick.Application.Initialization
{
    public class InitializeHandler : ICommandHandler<InitializeCommand>
    {
        private readonly InitializationResources resources;
        private readonly IMediator mediator;
        private readonly ServiceFactory serviceFactory;
        private readonly ISidekickSettings settings;
        private readonly ILogger<InitializeHandler> logger;
        private readonly IViewLocator viewLocator;
        private readonly IProcessProvider processProvider;
        private readonly IKeyboardProvider keyboardProvider;
        private readonly IScrollProvider scrollProvider;
        private readonly IMouseProvider mouseProvider;
        private readonly IScreenProvider screenProvider;
        private readonly IKeybindsExecutor keybindsExecutor;
        private readonly IKeybindProvider keybindProvider;

        public InitializeHandler(
            InitializationResources resources,
            IMediator mediator,
            ServiceFactory serviceFactory,
            ISidekickSettings settings,
            ILogger<InitializeHandler> logger,
            IViewLocator viewLocator,
            IProcessProvider processProvider,
            IKeyboardProvider keyboardProvider,
            IScrollProvider scrollProvider,
            IMouseProvider mouseProvider,
            IScreenProvider screenProvider,
            IKeybindsExecutor keybindsExecutor,
            IKeybindProvider keybindProvider)
        {
            this.resources = resources;
            this.mediator = mediator;
            this.serviceFactory = serviceFactory;
            this.settings = settings;
            this.logger = logger;
            this.viewLocator = viewLocator;
            this.processProvider = processProvider;
            this.keyboardProvider = keyboardProvider;
            this.scrollProvider = scrollProvider;
            this.mouseProvider = mouseProvider;
            this.screenProvider = screenProvider;
            this.keybindsExecutor = keybindsExecutor;
            this.keybindProvider = keybindProvider;
        }

        private int Count = 0;

        private int Completed = 0;

        private void AddNotificationCount<TNotification>(bool shouldAdd = true)
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
                Count = request.FirstRun ? 9 : 3;

                // Set the total count of handlers
                AddNotificationCount<DataInitializationStarted>();

                // Report initial progress
                await ReportProgress();

                // Open a clean view of the initialization
                viewLocator.CloseAll();
                if (settings.ShowSplashScreen)
                {
                    await viewLocator.Open(View.Initialization);
                }

                // Set the UI language
                await RunCommandStep(new SetUiLanguageCommand(settings.Language_UI));

                // Check for updates
                if (await mediator.Send(new IsNewVersionAvailableQuery()))
                {
                    await mediator.Send(new OpenConfirmNotificationCommand(resources.UpdateAvailable, resources.UpdateTitle, async () =>
                    {
                        await mediator.Send(new OpenBrowserCommand(new Uri("https://github.com/domialex/Sidekick/releases")));
                        await mediator.Send(new ShutdownCommand());
                    }));
                }

                // Check to see if we should run Setup first before running the rest of the initialization process
                if (string.IsNullOrEmpty(settings.LeagueId) || string.IsNullOrEmpty(settings.Language_Parser) || string.IsNullOrEmpty(settings.Language_UI))
                {
                    viewLocator.Close(View.Initialization);
                    await viewLocator.Open(View.Setup);
                    return Unit.Value;
                }

                // Set the game language
                await RunCommandStep(new SetGameLanguageCommand(settings.Language_Parser));

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
                        await mediator.Send(new OpenNotificationCommand(resources.NewLeagues));
                        viewLocator.Close(View.Initialization);
                        await viewLocator.Open(View.Setup);
                        return Unit.Value;
                    }
                }

                await RunNotificationStep(new DataInitializationStarted());

                if (request.FirstRun)
                {
                    await Run(() => processProvider.Initialize(cancellationToken));
                    await Run(() => keyboardProvider.Initialize());
                    await Run(() => scrollProvider.Initialize());
                    await Run(() => mouseProvider.Initialize());
                    await Run(() => screenProvider.Initialize());
                    await Run(() => keybindsExecutor.Initialize());
                    await Run(() => keybindProvider.Initialize());
                }

                // If we have a successful initialization, we delay for half a second to show the "Ready" label on the UI before closing the view
                Completed = Count;
                await ReportProgress();
                await Task.Delay(500);

                // Show a system notification
                await mediator.Send(new OpenNotificationCommand(string.Format(resources.Notification_Message, settings.Price_Key_Check.ToKeybindString(), settings.Price_Key_Close.ToKeybindString()),
                                                                resources.Notification_Title));

                viewLocator.Close(View.Initialization);

                return Unit.Value;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                await mediator.Send(new OpenNotificationCommand(resources.Error));
                await mediator.Send(new ShutdownCommand());
                return Unit.Value;
            }
        }

        private async Task Run(Func<Task> func)
        {
            // Send the command
            await func.Invoke();

            // Make sure that after all handlers run, the Completed count is updated
            Completed += 1;

            // Report progress
            await ReportProgress();
        }

        private async Task Run(Action action)
        {
            // Send the command
            action.Invoke();

            // Make sure that after all handlers run, the Completed count is updated
            Completed += 1;

            // Report progress
            await ReportProgress();
        }

        private async Task RunNotificationStep<TNotification>(TNotification notification, bool shouldRun = true)
            where TNotification : INotification
        {
            if (!shouldRun) return;

            // Publish the notification
            await mediator.Publish(notification);

            // Make sure that after all handlers run, the Completed count is updated
            Completed += serviceFactory.GetInstances<INotificationHandler<TNotification>>().Count();

            // Report progress
            await ReportProgress();
        }

        private async Task RunCommandStep<TCommand>(TCommand command, bool shouldRun = true)
            where TCommand : ICommand
        {
            if (!shouldRun) return;

            // Send the command
            await mediator.Send(command);

            // Make sure that after all handlers run, the Completed count is updated
            Completed += 1;

            // Report progress
            await ReportProgress();
        }

        private async Task ReportProgress()
        {
            var args = new InitializationProgressed(Count == 0 ? 0 : (Completed) * 100 / (Count));
            if (args.Percentage >= 100)
            {
                args.Title = resources.Ready;
                args.Percentage = 100;
            }
            else
            {
                args.Title = resources.Title(Completed, Count);
            }

            await mediator.Publish(args);
        }
    }
}
