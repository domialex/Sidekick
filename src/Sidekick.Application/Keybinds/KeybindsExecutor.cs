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
        private readonly IKeyboardProvider keyboard;
        private readonly IMediator mediator;
        private readonly ISidekickSettings settings;
        private readonly IProcessProvider processProvider;
        private readonly IScrollProvider scrollProvider;

        public KeybindsExecutor(
            ILogger<KeybindsExecutor> logger,
            IKeyboardProvider keyboard,
            IMediator mediator,
            ISidekickSettings settings,
            IProcessProvider processProvider,
            IScrollProvider scrollProvider)
        {
            this.logger = logger;
            this.keyboard = keyboard;
            this.mediator = mediator;
            this.settings = settings;
            this.processProvider = processProvider;
            this.scrollProvider = scrollProvider;
        }

        public bool Enabled { get; set; }

        public void Initialize()
        {
            keyboard.OnKeyDown += KeybindsProvider_OnKeyDown;
            scrollProvider.OnScrollDown += KeybindsProvider_OnScrollDown;
            scrollProvider.OnScrollUp += KeybindsProvider_OnScrollUp;
            Enabled = true;
        }

        private bool KeybindsProvider_OnScrollUp()
        {
            if (!processProvider.IsPathOfExileInFocus || !settings.Stash_EnableCtrlScroll || !keyboard.IsCtrlPressed())
            {
                return false;
            }

            Task.Run(() => mediator.Send(new ScrollStashUpCommand()));
            return true;
        }

        private bool KeybindsProvider_OnScrollDown()
        {
            if (!processProvider.IsPathOfExileInFocus || !settings.Stash_EnableCtrlScroll || !keyboard.IsCtrlPressed())
            {
                return false;
            }

            Task.Run(() => mediator.Send(new ScrollStashDownCommand()));
            return true;
        }

        private bool KeybindsProvider_OnKeyDown(KeyDownArgs args)
        {
            if (!Enabled || (!processProvider.IsPathOfExileInFocus && !processProvider.IsSidekickInFocus))
            {
                return false;
            }

            Enabled = false;

            Task<bool> task = null;

            // Chat commands
            ExecuteKeybind<ExitToCharacterSelectionCommand>(settings.Chat_Key_Exit, args, ref task, true);
            ExecuteKeybind<GoToHideoutCommand>(settings.Chat_Key_Hideout, args, ref task, true);
            ExecuteKeybind<LeavePartyCommand>(settings.Chat_Key_LeaveParty, args, ref task, true);
            ExecuteKeybind<ReplyToLastWhisperCommand>(settings.Chat_Key_ReplyToLastWhisper, args, ref task, true);

            // View commands
            ExecuteKeybind<CloseAllViewCommand>("Esc", args, ref task, false);
            ExecuteKeybind<ClosePriceViewCommand>(settings.Price_Key_Close, args, ref task, false);
            ExecuteKeybind<CloseMapViewCommand>(settings.Map_Key_Close, args, ref task, false);
            ExecuteKeybind<ToggleCheatsheetsCommand>(settings.Cheatsheets_Key_Open, args, ref task, true);
            ExecuteKeybind<OpenSettingsCommand>(settings.Key_OpenSettings, args, ref task, true);
            ExecuteKeybind<OpenMapInfoCommand>(settings.Map_Key_Check, args, ref task, true);
            ExecuteKeybind<PriceCheckItemCommand>(settings.Price_Key_Check, args, ref task, true);

            // Webpages
            ExecuteKeybind<OpenWikiPageCommand>(settings.Wiki_Key_Open, args, ref task, true);
            ExecuteKeybind<OpenTradePageCommand>(settings.Price_Key_OpenSearch, args, ref task, true);

            // Game commands
            ExecuteKeybind<ScrollStashUpCommand>(settings.Stash_Key_Left, args, ref task, true);
            ExecuteKeybind<ScrollStashDownCommand>(settings.Stash_Key_Right, args, ref task, true);
            ExecuteKeybind<FindItemCommand>(settings.Key_FindItems, args, ref task, true);

            foreach (var customChat in settings.Chat_CustomCommands)
            {
                ExecuteCustomChat(customChat, args, ref task);
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

                    if (!result && args.Intercepted)
                    {
                        Enabled = false;
                        await keyboard.PressKey(args.Key);
                    }

                    Enabled = true;
                });
            }

            return task != null;
        }

        private void ExecuteCustomChat(CustomChatSetting customChatSetting, KeyDownArgs args, ref Task<bool> returnTask)
        {
            if (!processProvider.IsPathOfExileInFocus)
            {
                return;
            }

            if (args.Key == customChatSetting.Key)
            {
                logger.LogInformation($"Keybind detected: {customChatSetting.Key}! Executing the custom chat command '{customChatSetting.ChatCommand}'");

                returnTask = mediator.Send(new CustomChatCommand(customChatSetting.ChatCommand));
            }
        }

        private void ExecuteKeybind<TCommand>(string keybind, KeyDownArgs args, ref Task<bool> returnTask, bool requireGameInFocus)
            where TCommand : ICommand<bool>, new()
        {
            if (requireGameInFocus && !processProvider.IsPathOfExileInFocus)
            {
                return;
            }

            if (args.Key == keybind)
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
            keyboard.OnKeyDown -= KeybindsProvider_OnKeyDown;
            scrollProvider.OnScrollDown -= KeybindsProvider_OnScrollDown;
            scrollProvider.OnScrollUp -= KeybindsProvider_OnScrollUp;
        }
    }
}
