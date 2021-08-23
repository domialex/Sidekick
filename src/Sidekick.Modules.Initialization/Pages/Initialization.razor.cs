using System;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sidekick.Apis.GitHub;
using Sidekick.Apis.Poe;
using Sidekick.Apis.Poe.Modifiers;
using Sidekick.Apis.Poe.Parser.Patterns;
using Sidekick.Apis.Poe.Pseudo;
using Sidekick.Common;
using Sidekick.Common.Blazor.Views;
using Sidekick.Common.Cache;
using Sidekick.Common.Game.Languages;
using Sidekick.Common.Localization;
using Sidekick.Common.Platform;
using Sidekick.Common.Settings;
using Sidekick.Core.Settings;
using Sidekick.Modules.Initialization.Localization;

namespace Sidekick.Modules.Initialization.Pages
{
    public partial class Initialization : ComponentBase
    {
        [Inject] private InitializationResources Resources { get; set; }
        [Inject] private ISettings Settings { get; set; }
        [Inject] private ISettingsService SettingsService { get; set; }
        [Inject] private ILogger<Initialization> Logger { get; set; }
        [Inject] private IViewLocator ViewLocator { get; set; }
        [Inject] private IProcessProvider ProcessProvider { get; set; }
        [Inject] private IKeyboardProvider KeyboardProvider { get; set; }
        [Inject] private IKeybindProvider KeybindProvider { get; set; }
        [Inject] private IParserPatterns ParserPatterns { get; set; }
        [Inject] private IModifierProvider ModifierProvider { get; set; }
        [Inject] private IPseudoModifierProvider PseudoModifierProvider { get; set; }
        [Inject] private IItemMetadataProvider ItemMetadataProvider { get; set; }
        [Inject] private IItemStaticDataProvider ItemStaticDataProvider { get; set; }
        [Inject] private IGitHubClient GitHubClient { get; set; }
        [Inject] private IGameLanguageProvider GameLanguageProvider { get; set; }
        [Inject] private IAppService AppService { get; set; }
        [Inject] private ICacheProvider CacheProvider { get; set; }
        [Inject] private IUILanguageProvider UILanguageProvider { get; set; }
        [Inject] private ILeagueProvider LeagueProvider { get; set; }

        private int Count { get; set; } = 0;
        private int Completed { get; set; } = 0;
        private string Title { get; set; }
        private int Percentage { get; set; }

        public static bool HasRun { get; set; } = false;

        protected override void OnInitialized()
        {
            Task.Run(Handle);
            base.OnInitialized();
        }

        public async Task Handle()
        {
            try
            {
                Completed = 0;
                Count = HasRun ? 7 : 10;

                // Report initial progress
                await ReportProgress();

                // Open a clean view of the initialization
                ViewLocator.CloseAll();
                if (Settings.ShowSplashScreen)
                {
                    await ViewLocator.Open(View.Initialization);
                }

                // Set the UI language
                await Run(() => UILanguageProvider.Set(Settings.Language_UI));

                // Check for updates
                if (!HasRun && await GitHubClient.Update())
                {
                    ViewLocator.Close(View.Initialization);
                    return;
                }

                // Check to see if we should run Setup first before running the rest of the initialization process
                if (string.IsNullOrEmpty(Settings.LeagueId) || string.IsNullOrEmpty(Settings.Language_Parser) || string.IsNullOrEmpty(Settings.Language_UI))
                {
                    ViewLocator.Close(View.Initialization);
                    await ViewLocator.Open(View.Setup);
                    return;
                }

                // Set the game language
                await Run(() => GameLanguageProvider.SetLanguage(Settings.Language_Parser));

                if (!HasRun)
                {
                    var leagues = await LeagueProvider.GetList(false);
                    var leaguesHash = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(leagues)));

                    if (leaguesHash != Settings.LeaguesHash)
                    {
                        CacheProvider.Clear();
                        await SettingsService.Save(nameof(ISettings.LeaguesHash), leaguesHash);
                    }

                    // Check to see if we should run Setup first before running the rest of the initialization process
                    if (string.IsNullOrEmpty(Settings.LeagueId) || !leagues.Any(x => x.Id == Settings.LeagueId))
                    {
                        await AppService.OpenNotification(Resources.NewLeagues);
                        ViewLocator.Close(View.Initialization);
                        await ViewLocator.Open(View.Setup);
                        return;
                    }
                }

                await Run(() => ParserPatterns.Initialize());
                await Run(() => ItemMetadataProvider.Initialize());
                await Run(() => ItemStaticDataProvider.Initialize());
                await Run(() => ModifierProvider.Initialize());
                await Run(() => PseudoModifierProvider.Initialize());

                if (!HasRun)
                {
                    await Run(() => ProcessProvider.Initialize());
                    await Run(() => KeyboardProvider.Initialize());
                    await Run(() => KeybindProvider.Initialize());
                }

                // If we have a successful initialization, we delay for half a second to show the "Ready" label on the UI before closing the view
                Completed = Count;
                await ReportProgress();
                await Task.Delay(500);

                // Show a system notification
                await AppService.OpenNotification(string.Format(Resources.Notification_Message, Settings.Trade_Key_Check.ToKeybindString(), Settings.Key_Close.ToKeybindString()),
                                                  Resources.Notification_Title);

                HasRun = true;
                ViewLocator.Close(View.Initialization);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
                await AppService.OpenNotification(Resources.Error);
                AppService.Shutdown();
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

        private Task ReportProgress(int percentage, string title)
        {
            Percentage = percentage;
            Title = title;

            StateHasChanged();
            return Task.Delay(100);
        }

        private Task ReportProgress()
        {
            Percentage = Count == 0 ? 0 : Completed * 100 / Count;
            if (Percentage >= 100)
            {
                Title = Resources.Ready;
                Percentage = 100;
            }
            else
            {
                Title = Resources.Title(Completed, Count);
            }

            StateHasChanged();
            return Task.Delay(100);
        }

        public void Exit()
        {
            AppService.Shutdown();
        }
    }
}
