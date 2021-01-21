using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Cheatsheets.Commands;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Game.Maps.Commands;
using Sidekick.Domain.Game.Shortcuts.Commands;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Game.Trade.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views.Commands;
using Sidekick.Domain.Wikis.Commands;

namespace Sidekick.Application.Keybinds
{
    public class KeybindsExecutor : IKeybindsExecutor, IDisposable
    {
        private readonly ILogger<KeybindsExecutor> logger;
        private readonly IKeybindsProvider keybindsProvider;
        private readonly IMediator mediator;
        private readonly ISidekickSettings settings;
        private readonly IProcessProvider processProvider;
        private readonly IScrollProvider scrollProvider;

        public KeybindsExecutor(
            ILogger<KeybindsExecutor> logger,
            IKeybindsProvider keybindsProvider,
            IMediator mediator,
            ISidekickSettings settings,
            IProcessProvider processProvider,
            IScrollProvider scrollProvider)
        {
            this.logger = logger;
            this.keybindsProvider = keybindsProvider;
            this.mediator = mediator;
            this.settings = settings;
            this.processProvider = processProvider;
            this.scrollProvider = scrollProvider;
        }

        public bool Enabled { get; set; }

        public void Initialize()
        {
            keybindsProvider.OnKeyDown += KeybindsProvider_OnKeyDown;
            scrollProvider.OnScrollDown += KeybindsProvider_OnScrollDown;
            scrollProvider.OnScrollUp += KeybindsProvider_OnScrollUp;
            Enabled = true;
        }

        private bool KeybindsProvider_OnScrollUp()
        {
            if (!processProvider.IsPathOfExileInFocus || !settings.Stash_EnableCtrlScroll || !keybindsProvider.IsCtrlPressed())
            {
                return false;
            }

            Task.Run(() => mediator.Send(new ScrollStashUpCommand()));
            return true;
        }

        private bool KeybindsProvider_OnScrollDown()
        {
            if (!processProvider.IsPathOfExileInFocus || !settings.Stash_EnableCtrlScroll || !keybindsProvider.IsCtrlPressed())
            {
                return false;
            }

            Task.Run(() => mediator.Send(new ScrollStashDownCommand()));
            return true;
        }

        private bool KeybindsProvider_OnKeyDown(string arg)
        {
            if (!Enabled || (!processProvider.IsPathOfExileInFocus && !processProvider.IsSidekickInFocus))
            {
                return false;
            }

            Enabled = false;

            Task<bool> task = null;

            // Chat commands
            ExecuteKeybind<ExitToCharacterSelectionCommand>(settings.Chat_Key_Exit, arg, ref task, true);
            ExecuteKeybind<GoToHideoutCommand>(settings.Chat_Key_Hideout, arg, ref task, true);
            ExecuteKeybind<LeavePartyCommand>(settings.Chat_Key_LeaveParty, arg, ref task, true);
            ExecuteKeybind<ReplyToLastWhisperCommand>(settings.Chat_Key_ReplyToLastWhisper, arg, ref task, true);

            // View commands
            ExecuteKeybind<CloseAllViewCommand>("Escape", arg, ref task, false);
            ExecuteKeybind<ClosePriceViewCommand>(settings.Price_Key_Close, arg, ref task, false);
            ExecuteKeybind<CloseMapViewCommand>(settings.Map_Key_Close, arg, ref task, false);
            ExecuteKeybind<ToggleCheatsheetsCommand>(settings.Cheatsheets_Key_Open, arg, ref task, true);
            ExecuteKeybind<OpenSettingsCommand>(settings.Key_OpenSettings, arg, ref task, true);
            ExecuteKeybind<OpenMapInfoCommand>(settings.Map_Key_Check, arg, ref task, true);
            ExecuteKeybind<PriceCheckItemCommand>(settings.Price_Key_Check, arg, ref task, true);

            // Webpages
            ExecuteKeybind<OpenWikiPageCommand>(settings.Wiki_Key_Open, arg, ref task, true);
            ExecuteKeybind<OpenTradePageCommand>(settings.Price_Key_OpenSearch, arg, ref task, true);

            // Game commands
            ExecuteKeybind<ScrollStashUpCommand>(settings.Stash_Key_Left, arg, ref task, true);
            ExecuteKeybind<ScrollStashDownCommand>(settings.Stash_Key_Right, arg, ref task, true);
            ExecuteKeybind<FindItemCommand>(settings.Key_FindItems, arg, ref task, true);

            foreach (var customChat in settings.Chat_CustomCommands)
            {
                ExecuteCustomChat(customChat, arg, ref task);
            }

            if (task == null)
            {
                Enabled = true;
            }
            else
            {
                // We need to make sure some key combinations make it into the game no matter what
                Task.Run(async () =>
                {
                    var result = await task;

                    if (!result)
                    {
                        Enabled = false;
                        await keybindsProvider.PressKey(arg);
                    }

                    Enabled = true;
                });
            }

            return task != null;
        }

        private void ExecuteCustomChat(CustomChatSetting customChatSetting, string input, ref Task<bool> returnTask)
        {
            if (!processProvider.IsPathOfExileInFocus)
            {
                return;
            }

            if (input == customChatSetting.Key)
            {
                logger.LogInformation($"Keybind detected: {customChatSetting.Key}! Executing the custom chat command '{customChatSetting.ChatCommand}'");

                returnTask = mediator.Send(new CustomChatCommand(customChatSetting.ChatCommand));
            }
        }

        private void ExecuteKeybind<TCommand>(string keybind, string input, ref Task<bool> returnTask, bool requireGameInFocus)
            where TCommand : ICommand<bool>, new()
        {
            if (requireGameInFocus && !processProvider.IsPathOfExileInFocus)
            {
                return;
            }

            if (input == keybind)
            {
                logger.LogInformation($"Keybind detected: {keybind}! Executing the command {typeof(TCommand).Name}.");

                var task = mediator.Send(new TCommand());

                if (returnTask == null)
                {
                    returnTask = task;
                }
                else
                {
                    var existingTask = returnTask;
                    returnTask = Task.Run(async () =>
                    {
                        var result = await Task.WhenAll(task, existingTask);
                        return result.Any(x => x);
                    });
                }
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
            scrollProvider.OnScrollDown -= KeybindsProvider_OnScrollDown;
            scrollProvider.OnScrollUp -= KeybindsProvider_OnScrollUp;
        }
    }
}
