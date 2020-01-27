using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Settings;
using Sidekick.Platforms.Windows.Natives.Helpers;
using WindowsHook;

namespace Sidekick.Platforms.Windows.Natives
{
    public class NativeKeyboard : INativeKeyboard, IOnAfterInit, IDisposable
    {
        private readonly ILogger logger;
        private readonly INativeProcess nativeProcess;
        private readonly SidekickSettings configuration;

        public NativeKeyboard(ILogger logger,
            INativeProcess nativeProcess,
            SidekickSettings configuration)
        {
            this.logger = logger;
            this.nativeProcess = nativeProcess;
            this.configuration = configuration;
        }

        public bool Enabled { get; set; }

        public event Func<string, Task> OnKeyDown;

        private IKeyboardMouseEvents hook = null;

        public Task OnAfterInit()
        {
            hook = Hook.GlobalEvents();
            hook.KeyDown += Hook_KeyDown;

            return Task.CompletedTask;
        }

        private void Hook_KeyDown(object sender, WindowsHook.KeyEventArgs e)
        {
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

                if (OnKeyDown != null) await OnKeyDown.Invoke(str.ToString());
            });
        }

        public void Dispose()
        {
            hook.KeyDown -= Hook_KeyDown;
            hook.Dispose();
        }

        public void SendCommand(KeyboardCommandEnum command)
        {
            switch (command)
            {
                case KeyboardCommandEnum.Copy:
                    SendKeys.SendWait("^{c}");
                    break;
                case KeyboardCommandEnum.FindItems:
                    SendKeys.SendWait("^{f}^{a}^{v}{Enter}");
                    break;
                case KeyboardCommandEnum.Stash_Left:
                    SendKeys.SendWait("{Left}");
                    break;
                case KeyboardCommandEnum.Stash_Right:
                    SendKeys.SendWait("{Right}");
                    break;
                case KeyboardCommandEnum.GoToHideout:

                    SendKeys.SendWait("{Enter}/hideout{Enter}{Enter}{Up}{Up}{Esc}");
                    break;
                case KeyboardCommandEnum.LeaveParty:
                    // This operation is only valid if the user has added their character name to the settings file.
                    if (string.IsNullOrEmpty(configuration.CharacterName))
                    {
                        logger.Log(@"This command requires a ""CharacterName"" to be specified in the settings menu.", LogState.Warning);
                        return;
                    }
                    SendKeys.SendWait($"{{Enter}}/kick {configuration.CharacterName}{{Enter}}");
                    break;
            }
        }

        public bool IsKeyPressed(string key)
        {
            switch (key)
            {
                case "Ctrl":
                    return Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_CONTROL)
                        || Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_LCONTROL)
                        || Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_RCONTROL);
                default:
                    throw new Exception("Unrecognized key.");
            }
        }
    }
}
