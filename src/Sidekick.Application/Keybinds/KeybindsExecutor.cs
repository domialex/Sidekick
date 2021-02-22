using System;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Stashes.Commands;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;

namespace Sidekick.Application.Keybinds
{
    public class KeybindsExecutor : IKeybindsExecutor, IDisposable
    {
        private readonly IMediator mediator;
        private readonly ISidekickSettings settings;
        private readonly IProcessProvider processProvider;
        private readonly IScrollProvider scrollProvider;
        private readonly IKeyboardProvider keyboard;

        public KeybindsExecutor(
            IMediator mediator,
            ISidekickSettings settings,
            IProcessProvider processProvider,
            IScrollProvider scrollProvider,
            IKeyboardProvider keyboard)
        {
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
