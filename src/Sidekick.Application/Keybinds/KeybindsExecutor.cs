using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Cheatsheets.Commands;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Application.Keybinds
{
    public class KeybindsExecutor : IKeybindsExecutor, IDisposable
    {
        private readonly ILogger<KeybindsExecutor> logger;
        private readonly IKeybindsProvider keybindsProvider;
        private readonly IMediator mediator;
        private readonly ISidekickSettings settings;

        public KeybindsExecutor(
            ILogger<KeybindsExecutor> logger,
            IKeybindsProvider keybindsProvider,
            IMediator mediator,
            ISidekickSettings settings)
        {
            this.logger = logger;
            this.keybindsProvider = keybindsProvider;
            this.mediator = mediator;
            this.settings = settings;
        }

        public bool Enabled { get; set; }

        public void Initialize()
        {
            keybindsProvider.OnKeyDown += KeybindsProvider_OnKeyDown;
            keybindsProvider.OnScrollDown += KeybindsProvider_OnScrollDown;
            keybindsProvider.OnScrollUp += KeybindsProvider_OnScrollUp;
            Enabled = true;
        }

        private bool KeybindsProvider_OnScrollUp()
        {
            Task.Run(() => mediator.Send(new ScrollStashUpCommand()));
            return true;
        }

        private bool KeybindsProvider_OnScrollDown()
        {
            Task.Run(() => mediator.Send(new ScrollStashDownCommand()));
            return true;
        }

        private bool KeybindsProvider_OnKeyDown(string arg)
        {
            if (!Enabled)
            {
                return false;
            }

            Enabled = false;

            Task<bool> task = null;

            // Chat commands
            ExecuteKeybind<ExitToCharacterSelectionCommand>(settings.Key_Exit, arg, ref task);
            ExecuteKeybind<GoToHideoutCommand>(settings.Key_GoToHideout, arg, ref task);
            ExecuteKeybind<LeavePartyCommand>(settings.Key_LeaveParty, arg, ref task);
            ExecuteKeybind<ReplyToLatestWhisperCommand>(settings.Key_ReplyToLatestWhisper, arg, ref task);

            // View commands
            ExecuteKeybind<CloseViewCommand>(settings.Key_CloseWindow, arg, ref task);
            ExecuteKeybind<ToggleCheatsheetsCommand>(settings.Key_OpenLeagueOverview, arg, ref task);
            ExecuteKeybind<OpenSettingsCommand>(settings.Key_OpenSettings, arg, ref task);
            // ExecuteKeybind(settings.Key_CheckPrices, request.Keys, OnPriceCheck, ref task);
            // ExecuteKeybind(settings.Key_MapInfo, request.Keys, OnMapInfo, ref task);
            // ExecuteKeybind(settings.Key_OpenWiki, request.Keys, OnItemWiki, ref task);
            // ExecuteKeybind(settings.Key_FindItems, request.Keys, OnFindItems, ref task);
            // ExecuteKeybind(settings.Key_OpenSearch, request.Keys, OnOpenSearch, ref task);

            // Game commands
            ExecuteKeybind<ScrollStashUpCommand>(settings.Key_Stash_Left, arg, ref task);
            ExecuteKeybind<ScrollStashDownCommand>(settings.Key_Stash_Right, arg, ref task);

            if (task == null)
            {
                Enabled = true;
            }
            else
            {
                Task.Run(async () =>
                {
                    await task;
                    Enabled = true;
                });
            }

            return task != null;
        }


        private void ExecuteKeybind<TCommand>(string keybind, string input, ref Task<bool> returnTask)
            where TCommand : ICommand<bool>, new()
        {
            if (input == keybind)
            {
                logger.LogInformation($"Keybind detected: {keybind}! Executing the command {typeof(TCommand).Name}.");

                var task = mediator.Send(new TCommand());

                // We need to make sure some key combinations make it into the game no matter what
                if (task != null && input == keybind)
                {
                    Task.Run(async () =>
                    {
                        if (!await task)
                        {
                            Enabled = false;
                            keybindsProvider.PressKey(keybind);
                            await Task.Delay(200);
                            Enabled = true;
                        }
                    });
                }

                returnTask = task;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            keybindsProvider.OnKeyDown -= KeybindsProvider_OnKeyDown;
            keybindsProvider.OnScrollDown -= KeybindsProvider_OnScrollDown;
            keybindsProvider.OnScrollUp -= KeybindsProvider_OnScrollUp;
        }
    }
}
