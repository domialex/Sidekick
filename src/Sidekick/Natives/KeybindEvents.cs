using System;
using System.Threading.Tasks;
using Serilog;
using Sidekick.Business.Stashes;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using WindowsHook;

namespace Sidekick.Natives
{
    public class KeybindEvents : IKeybindEvents, IOnAfterInit, IDisposable, IOnReset
    {
        private readonly ILogger logger;
        private readonly INativeProcess nativeProcess;
        private readonly SidekickSettings configuration;
        private readonly INativeKeyboard nativeKeyboard;
        private readonly IStashService stashService;
        private readonly HookProvider hookProvider;

        public KeybindEvents(ILogger logger,
            INativeProcess nativeProcess,
            SidekickSettings configuration,
            INativeKeyboard nativeKeyboard,
            IStashService stashService,
            HookProvider hookProvider)
        {
            this.logger = logger.ForContext(GetType());
            this.nativeProcess = nativeProcess;
            this.configuration = configuration;
            this.nativeKeyboard = nativeKeyboard;
            this.stashService = stashService;
            this.hookProvider = hookProvider;
        }

        public bool Enabled { get; set; }

        public event Func<Task<bool>> OnCloseWindow;
        public event Func<Task<bool>> OnExit;
        public event Func<Task<bool>> OnPriceCheck;
        public event Func<Task<bool>> OnHideout;
        public event Func<Task<bool>> OnItemWiki;
        public event Func<Task<bool>> OnFindItems;
        public event Func<Task<bool>> OnLeaveParty;
        public event Func<Task<bool>> OnOpenSearch;
        public event Func<Task<bool>> OnTabLeft;
        public event Func<Task<bool>> OnTabRight;
        public event Func<Task<bool>> OnOpenLeagueOverview;
        public event Func<Task<bool>> OnWhisperReply;

        private bool isDisposed;

        public Task OnAfterInit()
        {
            nativeKeyboard.OnKeyDown += NativeKeyboard_OnKeyDown;
            hookProvider.Hook.MouseWheelExt += Hook_MouseWheelExt;

            Enabled = true;

            return Task.CompletedTask;
        }

        private void Hook_MouseWheelExt(object sender, MouseEventExtArgs e)
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
                    stashService.ScrollLeft();
                }
                else
                {
                    stashService.ScrollRight();
                }

                e.Handled = true;
            }
        }

        private bool NativeKeyboard_OnKeyDown(string input)
        {
            if (Enabled && (nativeProcess.IsPathOfExileInFocus || nativeProcess.IsSidekickInFocus))
            {
                Enabled = false;

                Task<bool> task = null;

                ExecuteKeybind("Close Window", configuration.Key_CloseWindow, input, OnCloseWindow, ref task);
                ExecuteKeybind("Exit", configuration.Key_Exit, input, OnExit, ref task);
                ExecuteKeybind("Check Prices", configuration.Key_CheckPrices, input, OnPriceCheck, ref task);
                ExecuteKeybind("Open Wiki", configuration.Key_OpenWiki, input, OnItemWiki, ref task);
                ExecuteKeybind("Go to Hideout", configuration.Key_GoToHideout, input, OnHideout, ref task);
                ExecuteKeybind("Find Items", configuration.Key_FindItems, input, OnFindItems, ref task);
                ExecuteKeybind("Leave Party", configuration.Key_LeaveParty, input, OnLeaveParty, ref task);
                ExecuteKeybind("Open Search", configuration.Key_OpenSearch, input, OnOpenSearch, ref task);
                ExecuteKeybind("Open League Overview", configuration.Key_OpenLeagueOverview, input, OnOpenLeagueOverview, ref task);
                ExecuteKeybind("Scroll Tab Left", configuration.Key_Stash_Left, input, OnTabLeft, ref task);
                ExecuteKeybind("Scroll Tab Right", configuration.Key_Stash_Right, input, OnTabRight, ref task);
                ExecuteKeybind("Whisper Reply", configuration.Key_ReplyToLatestWhisper, input, OnWhisperReply, ref task);

                // We need to make sure some key combinations make it into the game if the keybind returns false
                SendInputIf("Ctrl+F", input, task);
                SendInputIf("Space", input, task);

                if (task == null)
                {
                    Enabled = true;
                }
                else
                {
                    Task.Run(async () =>
                    {
                        await task;
                        Enabled = true;
                    });
                }

                return task != null;
            }

            return false;
        }

        private void ExecuteKeybind(string name, string keybind, string input, Func<Task<bool>> func, ref Task<bool> returnTask)
        {
            if (input == keybind)
            {
                logger.Information("Keybind Triggered - {keybindName}", name);
                if (func != null)
                {
                    returnTask = func.Invoke();
                }
            }
        }

        // We need to make sure some key combinations make it into the game no matter what
        private void SendInputIf(string keybind, string input, Task<bool> task)
        {
            if (task != null && input == keybind)
            {
                Task.Run(async () =>
                {
                    if (!await task)
                    {
                        Enabled = false;
                        nativeKeyboard.SendInput(keybind);
                        await Task.Delay(200);
                        Enabled = true;
                    }
                });
            }
        }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            OnReset();
            isDisposed = true;
            GC.SuppressFinalize(this);
        }

        public void OnReset()
        {
            nativeKeyboard.OnKeyDown -= NativeKeyboard_OnKeyDown;
            if (hookProvider.Hook != null) // Hook will be null if auto update was successful
            {
                hookProvider.Hook.MouseWheelExt -= Hook_MouseWheelExt;
            }
        }
    }
}
