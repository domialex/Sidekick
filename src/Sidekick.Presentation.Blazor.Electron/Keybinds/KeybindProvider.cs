using System;
using System.Collections.Generic;
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

        private bool Registered { get; set; } = false;
        private Dictionary<string, List<Func<ICommand<bool>>>> Keybinds { get; set; }
            = new Dictionary<string, List<Func<ICommand<bool>>>>();

        public void Initialize()
        {
            processProvider.OnFocus += Register;
            processProvider.OnBlur += Unregister;
        }

        public void Register()
        {
            if (Registered)
            {
                Unregister();
            }

            Keybinds.Clear();

            RegisterKeybind<CloseAllViewCommand>("Esc");
            RegisterKeybind<CloseOverlayCommand>(settings.Key_Close);
            RegisterKeybind<FindItemCommand>(settings.Key_FindItems);
            RegisterKeybind<OpenSettingsCommand>(settings.Key_OpenSettings);

            RegisterKeybind<OpenCheatsheetsCommand>(settings.Cheatsheets_Key_Open);

            RegisterKeybind<OpenMapInfoCommand>(settings.Map_Key_Check);

            RegisterKeybind<PriceCheckItemCommand>(settings.Price_Key_Check);
            RegisterKeybind<OpenTradePageCommand>(settings.Price_Key_OpenSearch);

            RegisterKeybind<OpenWikiPageCommand>(settings.Wiki_Key_Open);

            RegisterKeybind<ScrollStashUpCommand>(settings.Stash_Key_Left);
            RegisterKeybind<ScrollStashDownCommand>(settings.Stash_Key_Right);

            foreach (var chat in settings.Chat_Commands)
            {
                RegisterChatKeybind(chat);
            }

            foreach (var keybind in Keybinds)
            {
                var accelerator = keyboardProvider.ToElectronAccelerator(keybind.Key);
                if (string.IsNullOrEmpty(accelerator))
                {
                    continue;
                }

                ElectronNET.API.Electron.GlobalShortcut.Register(accelerator, async () =>
                {
                    Unregister();
                    var handled = false;
                    foreach (var command in keybind.Value)
                    {
                        handled = await mediator.Send(command.Invoke()) || handled;
                    }
                    if (!handled && !Registered)
                    {
                        keyboardProvider.PressKey(keybind.Key);
                    }
                    Register();
                });
            }

            logger.LogDebug("[Keybind] Registered keybinds");
            Registered = true;
        }

        private void RegisterChatKeybind(ChatSetting chat)
        {
            if (string.IsNullOrEmpty(chat.Key))
            {
                return;
            }

            if (!Keybinds.ContainsKey(chat.Key))
            {
                Keybinds.Add(chat.Key, new List<Func<ICommand<bool>>>());
            }

            Keybinds[chat.Key].Add(() => new ChatCommand(chat.Command, chat.Submit));
        }

        private void RegisterKeybind<TCommand>(string keybind)
            where TCommand : ICommand<bool>, new()
        {
            if (string.IsNullOrEmpty(keybind))
            {
                return;
            }

            if (!Keybinds.ContainsKey(keybind))
            {
                Keybinds.Add(keybind, new List<Func<ICommand<bool>>>());
            }

            Keybinds[keybind].Add(() => new TCommand());
        }

        public void Unregister()
        {
            Registered = false;
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
