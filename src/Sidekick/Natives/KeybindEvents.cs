using System;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using WindowsHook;

namespace Sidekick.Natives
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
            Task.Run(async () =>
            {
                if (!Enabled || !configuration.CloseOverlayWithMouse)
                {
                    return;
                }

                if (OnMouseClick != null) await OnMouseClick.Invoke(e.X, e.Y);
            });
        }

        private void Hook_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            Task.Run(() =>
            {
                if (!Enabled || !configuration.EnableCtrlScroll || !nativeProcess.IsPathOfExileInFocus)
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

        private void Hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Enabled)
            {
                return;
            }

            Enabled = false;

            // Transfer the event key to a string to compare to settings.
            var str = new StringBuilder();
            if (e.Modifiers.HasFlag(Keys.Control))
            {
                str.Append("Ctrl+");
            }
            if (e.Modifiers.HasFlag(Keys.Shift))
            {
                str.Append("Shift+");
            }
            if (e.Modifiers.HasFlag(Keys.Alt))
            {
                str.Append("Alt+");
            }
            if (e.Modifiers.HasFlag(Keys.LWin))
            {
                str.Append("Win+");
            }
            str.Append(e.KeyCode);
            var key = str.ToString();

            if (key == configuration.KeyCloseWindow)
            {
                // TODO: Check if Overlay is opened and PoE in focus before using e.Handled here.
                //e.Handled = true;
                logger.Log("Keybind for closing the window triggered.");
                if (OnCloseWindow != null) Task.Run(OnCloseWindow);
            }

            if (nativeProcess.IsPathOfExileInFocus)
            {
                if (key == configuration.KeyPriceCheck)
                {
                    e.Handled = true;
                    logger.Log("Keybind for price checking triggered.");
                    if (OnPriceCheck != null) Task.Run(OnPriceCheck);
                }
                else if (key == configuration.KeyItemWiki)
                {
                    e.Handled = true;
                    logger.Log("Keybind for opening the item wiki triggered.");
                    if (OnItemWiki != null) Task.Run(OnItemWiki);
                }
                else if (key == configuration.KeyHideout)
                {
                    e.Handled = true;
                    logger.Log("Keybind for going to the hideout triggered.");
                    if (OnHideout != null) Task.Run(OnHideout);
                }
                else if (key == configuration.KeyFindItems)
                {
                    e.Handled = true;
                    logger.Log("Keybind for finding the item triggered.");
                    if (OnFindItems != null) Task.Run(OnFindItems);
                }
                else if (key == configuration.KeyLeaveParty)
                {
                    e.Handled = true;
                    logger.Log("Keybind for leaving the party triggered.");
                    if (OnLeaveParty != null) Task.Run(OnLeaveParty);
                }
                else if (key == configuration.KeyOpenSearch)
                {
                    e.Handled = true;
                    logger.Log("Keybind for opening the search triggered.");
                    if (OnOpenSearch != null) Task.Run(OnOpenSearch);
                }
                else if (key == configuration.KeyOpenLeagueOverview)
                {
                    e.Handled = true;
                    logger.Log("Keybind for opening the league overview triggered.");
                    if (OnOpenLeagueOverview != null) Task.Run(OnOpenLeagueOverview);
                }
            }

            Task.Run(async () =>
            {
                await Task.Delay(5);
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
