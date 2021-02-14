using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;

namespace Sidekick.Presentation.Blazor.Electron.Keybinds
{
    public class KeybindProvider : IKeybindProvider
    {
        private readonly ILogger<KeybindProvider> logger;
        private readonly ISidekickSettings settings;
        private readonly IKeyboardProvider keyboardProvider;
        private readonly IKeybindsExecutor keybindsExecutor;

        public KeybindProvider(
            ILogger<KeybindProvider> logger,
            ISidekickSettings settings,
            IKeyboardProvider keyboardProvider,
            IKeybindsExecutor keybindsExecutor)
        {
            this.logger = logger;
            this.settings = settings;
            this.keyboardProvider = keyboardProvider;
            this.keybindsExecutor = keybindsExecutor;
        }

        private bool Registered { get; set; } = false;

        public void Register()
        {
            if (Registered)
            {
                Unregister();
            }

            var keybinds = new List<string>()
            {
                "Esc",
                settings.Key_FindItems,
                settings.Key_OpenSettings,
                settings.Cheatsheets_Key_Open,
                settings.Map_Key_Check,
                settings.Map_Key_Close,
                settings.Price_Key_Check,
                settings.Price_Key_Close,
                settings.Price_Key_OpenSearch,
                settings.Stash_Key_Left,
                settings.Stash_Key_Right,
                settings.Wiki_Key_Open,
            };

            foreach (var chat in settings.Chat_Commands)
            {
                keybinds.Add(chat.Key);
            }

            foreach (var keybind in keybinds
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .Select(x => new
                {
                    Electron = keyboardProvider.ToElectronAccelerator(x),
                    Keybind = x,
                })
                .Where(x => !string.IsNullOrEmpty(x.Electron)))
            {
                ElectronNET.API.Electron.GlobalShortcut.Register(keybind.Electron, async () =>
                {
                    Unregister();
                    var result = await keybindsExecutor.Execute(keybind.Keybind);
                    if (!result && !Registered)
                    {
                        keyboardProvider.PressKey(keybind.Keybind);
                    }
                    Register();
                });
            }

            logger.LogDebug("[Keybind] Registered keybinds");
            Registered = true;
        }

        public void Unregister()
        {
            Registered = false;
            ElectronNET.API.Electron.GlobalShortcut.UnregisterAll();
            logger.LogDebug("[Keybind] Unregistered keybinds");
        }
    }
}
