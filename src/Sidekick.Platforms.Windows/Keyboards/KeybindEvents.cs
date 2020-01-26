using System;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Core.Configuration;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using WindowsHook;

namespace Sidekick.Platforms.Windows.Keyboards
{
    public class KeybindEvents : IKeybindEvents, IOnAfterInit, IDisposable
    {
        private readonly ILogger logger;
        private readonly INativeProcess nativeProcess;
        private readonly Configuration configuration;
        private readonly INativeKeyboard nativeKeyboard;

        public KeybindEvents(ILogger logger,
            INativeProcess nativeProcess,
            Configuration configuration,
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

        private IKeyboardMouseEvents hook = null;

        public async Task OnAfterInit()
        {
            await nativeProcess.CheckPermission();

            hook = Hook.GlobalEvents();
            hook.KeyDown += Hook_KeyDown;
            hook.MouseWheelExt += Hook_MouseWheelExt;
        }

        private void Hook_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            if (!Enabled)
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
                    Enabled = false;
                    logger.Log("Keybind for closing the window triggered.");
                    await OnCloseWindow?.Invoke();
                    Enabled = true;
                }

                if (nativeProcess.IsPathOfExileInFocus)
                {
                    if (key == configuration.KeyPriceCheck)
                    {
                        Enabled = false;
                        logger.Log("Keybind for price checking triggered.");
                        await OnPriceCheck?.Invoke();
                        Enabled = true;
                    }
                    else if (key == configuration.KeyItemWiki)
                    {
                        Enabled = false;
                        logger.Log("Keybind for opening the item wiki triggered.");
                        await OnItemWiki?.Invoke();
                        Enabled = true;
                    }
                    else if (key == configuration.KeyHideout)
                    {
                        Enabled = false;
                        logger.Log("Keybind for going to the hideout triggered.");
                        await OnHideout?.Invoke();
                        Enabled = true;
                    }
                    else if (key == configuration.KeyFindItems)
                    {
                        Enabled = false;
                        logger.Log("Keybind for finding the item triggered.");
                        await OnFindItems?.Invoke();
                        Enabled = true;
                    }
                    else if (key == configuration.KeyLeaveParty)
                    {
                        Enabled = false;
                        logger.Log("Keybind for leaving the party triggered.");
                        await OnLeaveParty?.Invoke();
                        Enabled = true;
                    }
                    else if (key == configuration.KeyOpenSearch)
                    {
                        Enabled = false;
                        logger.Log("Keybind for opening the search triggered.");
                        await OnOpenSearch?.Invoke();
                        Enabled = true;
                    }
                    else if (key == configuration.KeyOpenLeagueOverview)
                    {
                        Enabled = false;
                        logger.Log("Keybind for opening the league overview triggered.");
                        await OnOpenLeagueOverview?.Invoke();
                        Enabled = true;
                    }
                }
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
