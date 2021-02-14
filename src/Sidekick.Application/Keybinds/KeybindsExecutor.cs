using System;
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
        private readonly IMediator mediator;
        private readonly ISidekickSettings settings;
        private readonly IProcessProvider processProvider;
        private readonly IScrollProvider scrollProvider;
        private readonly IKeyboardProvider keyboard;

        public KeybindsExecutor(
            ILogger<KeybindsExecutor> logger,
            IMediator mediator,
            ISidekickSettings settings,
            IProcessProvider processProvider,
            IScrollProvider scrollProvider,
            IKeyboardProvider keyboard)
        {
            this.logger = logger;
            this.mediator = mediator;
            this.settings = settings;
            this.processProvider = processProvider;
            this.scrollProvider = scrollProvider;
            this.keyboard = keyboard;
        }

        public void Initialize()
        {
            scrollProvider.OnScrollDown += ScrollProvider_OnScrollDown;
            scrollProvider.OnScrollUp += ScrollProvider_OnScrollUp;
        }

        private bool ScrollProvider_OnScrollUp()
        {
            if (!processProvider.IsPathOfExileInFocus() || !settings.Stash_EnableCtrlScroll || !keyboard.IsCtrlPressed())
            {
                return false;
            }

            Task.Run(() => mediator.Send(new ScrollStashUpCommand()));
            return true;
        }

        private bool ScrollProvider_OnScrollDown()
        {
            if (!processProvider.IsPathOfExileInFocus() || !settings.Stash_EnableCtrlScroll || !keyboard.IsCtrlPressed())
            {
                return false;
            }

            Task.Run(() => mediator.Send(new ScrollStashDownCommand()));
            return true;
        }

        public async Task<bool> Execute(string keybind)
        {
            var poeInFocus = processProvider.IsPathOfExileInFocus();

            var handled = false;

            if (!poeInFocus && !processProvider.IsSidekickInFocus())
            {
                return handled;
            }

            // Close commands
            if (keybind == "Esc") handled = await mediator.Send(new CloseAllViewCommand());
            if (keybind == settings.Price_Key_Close) handled = await mediator.Send(new ClosePriceViewCommand()) || handled;
            if (keybind == settings.Map_Key_Close) handled = await mediator.Send(new CloseMapViewCommand()) || handled;

            if (!poeInFocus)
            {
                return handled;
            }

            // Commands below this point absolutely need to be inside of the game.

            // View commands
            if (keybind == settings.Cheatsheets_Key_Open) handled = await mediator.Send(new OpenCheatsheetsCommand()) || handled;
            if (keybind == settings.Key_OpenSettings) handled = await mediator.Send(new OpenSettingsCommand()) || handled;
            if (keybind == settings.Map_Key_Check) handled = await mediator.Send(new OpenMapInfoCommand()) || handled;
            if (keybind == settings.Price_Key_Check) handled = await mediator.Send(new PriceCheckItemCommand()) || handled;

            // Webpages
            if (keybind == settings.Wiki_Key_Open) handled = await mediator.Send(new OpenWikiPageCommand()) || handled;
            if (keybind == settings.Price_Key_OpenSearch) handled = await mediator.Send(new OpenTradePageCommand()) || handled;

            // Game commands
            if (keybind == settings.Stash_Key_Left) handled = await mediator.Send(new ScrollStashUpCommand()) || handled;
            if (keybind == settings.Stash_Key_Right) handled = await mediator.Send(new ScrollStashDownCommand()) || handled;
            if (keybind == settings.Key_FindItems) handled = await mediator.Send(new FindItemCommand()) || handled;

            // Chat commands
            foreach (var chatSetting in settings.Chat_Commands)
            {
                if (keybind == chatSetting.Key) handled = await mediator.Send(new ChatCommand(chatSetting.Command, chatSetting.Submit)) || handled;
            }

            return handled;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            scrollProvider.OnScrollDown -= ScrollProvider_OnScrollDown;
            scrollProvider.OnScrollUp -= ScrollProvider_OnScrollUp;
        }
    }
}
