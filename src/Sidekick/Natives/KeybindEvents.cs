using System;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Windows.Overlay;
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
        public event Func<Task> OnWhisperReply;
        public event Func<int, int, Task> OnMouseClick;

        private IKeyboardMouseEvents hook = null;

        public Task OnAfterInit()
        {
            nativeKeyboard.OnKeyDown += NativeKeyboard_OnKeyDown;
            hook = Hook.GlobalEvents();

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
                if (!configuration.EnableCtrlScroll || !nativeProcess.IsPathOfExileInFocus)
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

        private bool NativeKeyboard_OnKeyDown(string input)
        {
            if (!Enabled)
            {
                return false;
            }

            Enabled = false;

            var keybindFound = false;

            if (input == configuration.Key_CloseWindow)
            {
                if (OverlayController.IsDisplayed)
                {
                    keybindFound = true;
                }
                logger.Log("Keybind for closing the window triggered.");
                if (OnCloseWindow != null) Task.Run(OnCloseWindow);
            }

            if (nativeProcess.IsPathOfExileInFocus)
            {
                if (input == configuration.Key_CheckPrices)
                {
                    keybindFound = true;
                    logger.Log("Keybind for price checking triggered.");
                    if (OnPriceCheck != null) Task.Run(OnPriceCheck);
                }
                else if (input == configuration.Key_OpenWiki)
                {
                    keybindFound = true;
                    logger.Log("Keybind for opening the item wiki triggered.");
                    if (OnItemWiki != null) Task.Run(OnItemWiki);
                }
                else if (input == configuration.Key_GoToHideout)
                {
                    keybindFound = true;
                    logger.Log("Keybind for going to the hideout triggered.");
                    if (OnHideout != null) Task.Run(OnHideout);
                }
                else if (input == configuration.Key_FindItems)
                {
                    keybindFound = true;
                    logger.Log("Keybind for finding the item triggered.");
                    if (OnFindItems != null) Task.Run(OnFindItems);
                }
                else if (input == configuration.Key_LeaveParty)
                {
                    keybindFound = true;
                    logger.Log("Keybind for leaving the party triggered.");
                    if (OnLeaveParty != null) Task.Run(OnLeaveParty);
                }
                else if (input == configuration.Key_OpenSearch)
                {
                    keybindFound = true;
                    logger.Log("Keybind for opening the search triggered.");
                    if (OnOpenSearch != null) Task.Run(OnOpenSearch);
                }
                else if (input == configuration.Key_OpenLeagueOverview)
                {
                    keybindFound = true;
                    logger.Log("Keybind for opening the league overview triggered.");
                    if (OnOpenLeagueOverview != null) Task.Run(OnOpenLeagueOverview);
                }
                else if (input == configuration.Key_ReplyToLatestWhisper)
                {
                    keybindFound = true;
                    logger.Log("Keybind for replying to most recent whisper triggered.");
                    if (OnWhisperReply != null) Task.Run(OnWhisperReply);
                }
            }

            Task.Run(async () =>
            {
                await Task.Delay(500);
                Enabled = true;
            });

            // We need to make sure some key combinations make it into the game no matter what
            if (keybindFound)
            {
                keybindFound = input != "Ctrl+F";
            }

            return keybindFound;
        }

        public void Dispose()
        {
            nativeKeyboard.OnKeyDown -= NativeKeyboard_OnKeyDown;
            if (hook != null) // Hook will be null if auto update was successful
            {
                hook.MouseWheelExt -= Hook_MouseWheelExt;
                hook.Dispose();
            }
        }
    }
}
