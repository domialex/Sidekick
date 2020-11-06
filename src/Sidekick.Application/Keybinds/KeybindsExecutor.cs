using System;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Stashes;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Application.Keybinds
{
    public class KeybindsExecutor : IKeybindsExecutor, IDisposable
    {
        private readonly IKeybindsProvider keybindsProvider;
        private readonly IMediator mediator;
        private readonly ISidekickSettings settings;
        private readonly IStashService stashService;

        public KeybindsExecutor(
            IKeybindsProvider keybindsProvider,
            IMediator mediator,
            ISidekickSettings settings,
            IStashService stashService)
        {
            this.keybindsProvider = keybindsProvider;
            this.mediator = mediator;
            this.settings = settings;
            this.stashService = stashService;
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
            Task.Run(() => stashService.ScrollLeft());
            return true;
        }

        private bool KeybindsProvider_OnScrollDown()
        {
            Task.Run(() => stashService.ScrollRight());
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

            // View commands
            ExecuteKeybind<CloseViewCommand>(settings.Key_CloseWindow, arg, ref task);
            // ExecuteKeybind(settings.Key_CheckPrices, request.Keys, OnPriceCheck, ref task);
            // ExecuteKeybind(settings.Key_MapInfo, request.Keys, OnMapInfo, ref task);
            // ExecuteKeybind(settings.Key_OpenWiki, request.Keys, OnItemWiki, ref task);
            // ExecuteKeybind(settings.Key_FindItems, request.Keys, OnFindItems, ref task);
            // ExecuteKeybind(settings.Key_OpenSearch, request.Keys, OnOpenSearch, ref task);
            // ExecuteKeybind(settings.Key_OpenSettings, request.Keys, OnOpenSettings, ref task);
            // ExecuteKeybind(settings.Key_OpenLeagueOverview, request.Keys, OnOpenLeagueOverview, ref task);
            // ExecuteKeybind(settings.Key_Stash_Left, request.Keys, OnTabLeft, ref task);
            // ExecuteKeybind(settings.Key_Stash_Right, request.Keys, OnTabRight, ref task);
            // ExecuteKeybind(settings.Key_ReplyToLatestWhisper, request.Keys, OnWhisperReply, ref task);

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
            keybindsProvider.OnKeyDown -= KeybindsProvider_OnKeyDown;
            keybindsProvider.OnScrollDown -= KeybindsProvider_OnScrollDown;
            keybindsProvider.OnScrollUp -= KeybindsProvider_OnScrollUp;
        }
    }
}
