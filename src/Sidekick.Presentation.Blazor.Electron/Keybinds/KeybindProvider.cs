using System;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Cheatsheets.Commands;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Game.Maps.Commands;
using Sidekick.Domain.Game.Shortcuts.Commands;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Game.Trade.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Settings.Commands;
using Sidekick.Domain.Views.Commands;
using Sidekick.Domain.Wikis.Commands;

namespace Sidekick.Presentation.Blazor.Electron.Keybinds
{
    public class KeybindProvider : IKeybindProvider, IDisposable
    {
        private readonly IMediator mediator;
        private readonly ILogger<KeybindProvider> logger;
        private readonly ISidekickSettings settings;
        private readonly IKeyboardProvider keyboardProvider;
        private readonly IProcessProvider processProvider;

        public KeybindProvider(
            IMediator mediator,
            ILogger<KeybindProvider> logger,
            ISidekickSettings settings,
            IKeyboardProvider keyboardProvider,
            IProcessProvider processProvider)
        {
            this.mediator = mediator;
            this.logger = logger;
            this.settings = settings;
            this.keyboardProvider = keyboardProvider;
            this.processProvider = processProvider;
        }

        public void Initialize()
        {
            processProvider.OnFocus += Register;
            processProvider.OnBlur += Unregister;
        }

        public void Register()
        {
            Unregister();

            RegisterKeybind<CloseAllViewCommand>("Esc");
            RegisterKeybind<CloseOverlayCommand>(settings.Key_Close);
            RegisterKeybind<FindItemCommand>(settings.Key_FindItems);
            RegisterKeybind<OpenSettingsCommand>(settings.Key_OpenSettings);
            RegisterKeybind<OpenCheatsheetsCommand>(settings.Cheatsheets_Key_Open);
            RegisterKeybind<OpenMapInfoCommand>(settings.Map_Key_Check);
            RegisterKeybind<PriceCheckItemCommand>(settings.Trade_Key_Check);
            RegisterKeybind<OpenTradePageCommand>(settings.Trade_Key_OpenSearch);
            RegisterKeybind<OpenWikiPageCommand>(settings.Wiki_Key_Open);
            RegisterKeybind<ScrollStashUpCommand>(settings.Stash_Key_Left);
            RegisterKeybind<ScrollStashDownCommand>(settings.Stash_Key_Right);

            foreach (var chat in settings.Chat_Commands)
            {
                RegisterChatKeybind(chat);
            }

            logger.LogDebug("[Keybind] Registered keybinds");
        }

        private Action GetAction(string accelerator, string key, Func<ICommand<bool>> keybind)
        {
            return async () =>
            {
                ElectronNET.API.Electron.GlobalShortcut.Unregister(accelerator);
                var handled = await mediator.Send(keybind.Invoke());
                if (!handled)
                {
                    keyboardProvider.PressKey(key);
                }
                ElectronNET.API.Electron.GlobalShortcut.Register(accelerator, GetAction(accelerator, key, keybind));
            };
        }

        private void RegisterKeybind<TCommand>(string keybind)
            where TCommand : ICommand<bool>, new()
        {
            if (string.IsNullOrEmpty(keybind)) return;

            var accelerator = keyboardProvider.ToElectronAccelerator(keybind);
            if (string.IsNullOrEmpty(accelerator)) return;

            ElectronNET.API.Electron.GlobalShortcut.Register(accelerator, GetAction(accelerator, keybind, () => new TCommand()));
        }

        private void RegisterChatKeybind(ChatSetting chat)
        {
            if (string.IsNullOrEmpty(chat.Key)) return;

            var accelerator = keyboardProvider.ToElectronAccelerator(chat.Key);
            if (string.IsNullOrEmpty(accelerator)) return;

            ElectronNET.API.Electron.GlobalShortcut.Register(accelerator, GetAction(accelerator, chat.Key, () => new ChatCommand(chat.Command, chat.Submit)));
        }

        public void Unregister()
        {
            ElectronNET.API.Electron.GlobalShortcut.UnregisterAll();
            logger.LogDebug("[Keybind] Unregistered keybinds");
        }

        public void Dispose()
        {
            processProvider.OnFocus -= Register;
            processProvider.OnBlur -= Unregister;
        }
    }
}
