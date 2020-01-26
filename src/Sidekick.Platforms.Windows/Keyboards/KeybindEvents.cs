using System;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Settings;
using WindowsHook;

namespace Sidekick.Platforms.Windows.Keyboards
{
    public class KeybindEvents : IKeybindEvents, IOnAfterInit, IDisposable
    {
        private readonly ILogger logger;
        private readonly INativeProcess nativeProcess;
        private readonly SidekickSettings configuration;
        private readonly INativeKeyboard nativeKeyboard;

        public KeybindEvents(ILogger logger,
            INativeProcess nativeProcess,
            SidekickSettings configuration,
            INativeKeyboard nativeKeyboard)
        {
            this.logger = logger;
            this.nativeProcess = nativeProcess;
            this.configuration = configuration;
            this.nativeKeyboard = nativeKeyboard;
        }

        public bool Enabled { get; set; }

        public event Func<Task> OnCloseWindow;
        public event Func<Task> OnPriceCheck;
        public event Func<Task> OnHideout;
        public event Func<Task> OnItemWiki;
        public event Func<Task> OnFindItems;
        public event Func<Task> OnLeaveParty;
        public event Func<Task> OnOpenSearch;
        public event Func<Task> OnOpenLeagueOverview;
        public event Func<int, int, Task> OnMouseClick;

        private IKeyboardMouseEvents hook = null;

        public Task OnAfterInit()
        {
            hook = Hook.GlobalEvents();
            hook.KeyDown += Hook_KeyDown;

#if !DEBUG  
            hook.MouseWheelExt += Hook_MouseWheelExt;
            hook.MouseUp += Hook_MouseUp;
#endif

            Enabled = true;

            return Task.CompletedTask;
        }

        private void Hook_MouseUp(object sender, MouseEventArgs e)
        {
            if (!Enabled || !configuration.CloseOverlayWithMouse) return;

            Task.Run(async () =>
            {
                await OnMouseClick?.Invoke(e.X, e.Y);
            });
        }

        private void Hook_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            if (!Enabled || !configuration.EnableCtrlScroll)
            {
                return;
            }

            Task.Run(() =>
            {
                if (!nativeProcess.IsPathOfExileInFocus)
                {
                    return;
                }

                // Ctrl + Scroll wheel to move between stash tabs.
                if (nativeKeyboard.IsKeyPressed("Ctrl"))
                {
                    if (e.Delta > 0)
                    {
                        nativeKeyboard.SendCommand(KeyboardCommandEnum.Stash_Left);
                    }
                    else
                    {
                        nativeKeyboard.SendCommand(KeyboardCommandEnum.Stash_Right);
                    }
                }
            });
        }

        private void Hook_KeyDown(object sender, WindowsHook.KeyEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            Enabled = false;

            Task.Run(async () =>
            {
                // Transfer the event key to a string to compare to settings
                var str = new StringBuilder();
                if (e.Modifiers.HasFlag(WindowsHook.Keys.Control))
                {
                    str.Append("Ctrl+");
                }
                if (e.Modifiers.HasFlag(WindowsHook.Keys.Shift))
                {
                    str.Append("Shift+");
                }
                if (e.Modifiers.HasFlag(WindowsHook.Keys.Alt))
                {
                    str.Append("Alt+");
                }
                if (e.Modifiers.HasFlag(WindowsHook.Keys.LWin))
                {
                    str.Append("Win+");
                }
                str.Append(e.KeyCode);
                var key = str.ToString();

                if (key == configuration.KeyCloseWindow)
                {
                    e.Handled = true;
                    logger.Log("Keybind for closing the window triggered.");
                    await OnCloseWindow?.Invoke();
                }

                if (nativeProcess.IsPathOfExileInFocus)
                {
                    if (key == configuration.KeyPriceCheck)
                    {
                        e.Handled = true;
                        logger.Log("Keybind for price checking triggered.");
                        await OnPriceCheck?.Invoke();
                    }
                    else if (key == configuration.KeyItemWiki)
                    {
                        e.Handled = true;
                        logger.Log("Keybind for opening the item wiki triggered.");
                        await OnItemWiki?.Invoke();
                    }
                    else if (key == configuration.KeyHideout)
                    {
                        e.Handled = true;
                        logger.Log("Keybind for going to the hideout triggered.");
                        await OnHideout?.Invoke();
                    }
                    else if (key == configuration.KeyFindItems)
                    {
                        e.Handled = true;
                        logger.Log("Keybind for finding the item triggered.");
                        await OnFindItems?.Invoke();
                    }
                    else if (key == configuration.KeyLeaveParty)
                    {
                        e.Handled = true;
                        logger.Log("Keybind for leaving the party triggered.");
                        await OnLeaveParty?.Invoke();
                    }
                    else if (key == configuration.KeyOpenSearch)
                    {
                        e.Handled = true;
                        logger.Log("Keybind for opening the search triggered.");
                        await OnOpenSearch?.Invoke();
                    }
                    else if (key == configuration.KeyOpenLeagueOverview)
                    {
                        e.Handled = true;
                        logger.Log("Keybind for opening the league overview triggered.");
                        await OnOpenLeagueOverview?.Invoke();
                    }
                }
            });

            Task.Run(async () =>
            {
                await Task.Delay(100);
                Enabled = true;
            });
        }

        public void Dispose()
        {
            hook.KeyDown -= Hook_KeyDown;
            hook.MouseWheelExt -= Hook_MouseWheelExt;
            hook.Dispose();
        }
    }
}
