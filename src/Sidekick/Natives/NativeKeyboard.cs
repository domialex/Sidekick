using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sidekick.Core.Initialization;
using Sidekick.Core.Loggers;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Natives.Helpers;
using WindowsHook;

namespace Sidekick.Natives
{
    public class NativeKeyboard : INativeKeyboard, IOnAfterInit, IDisposable
    {
        private static List<WindowsHook.Keys> KEYS_INVALID = new List<WindowsHook.Keys>() {
            WindowsHook.Keys.ControlKey,
            WindowsHook.Keys.LControlKey,
            WindowsHook.Keys.RControlKey,
            WindowsHook.Keys.ShiftKey,
            WindowsHook.Keys.RShiftKey,
            WindowsHook.Keys.LShiftKey,
            WindowsHook.Keys.RWin,
            WindowsHook.Keys.LWin,
            WindowsHook.Keys.LMenu,
            WindowsHook.Keys.RMenu,
        };

        private readonly ILogger logger;
        private readonly SidekickSettings configuration;

        public NativeKeyboard(ILogger logger,
            SidekickSettings configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public bool Enabled { get; set; }

        public event Func<string, bool> OnKeyDown;

        private IKeyboardMouseEvents hook = null;

        public Task OnAfterInit()
        {
            hook = Hook.GlobalEvents();
            hook.KeyDown += Hook_KeyDown;

            return Task.CompletedTask;
        }

        private void Hook_KeyDown(object sender, WindowsHook.KeyEventArgs e)
        {
            if (KEYS_INVALID.Contains(e.KeyCode))
            {
                return;
            }

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
            if (e.Modifiers.HasFlag(WindowsHook.Keys.LWin) || e.Modifiers.HasFlag(WindowsHook.Keys.RWin))
            {
                str.Append("Win+");
            }

            str.Append(e.KeyCode);

            if (OnKeyDown != null)
            {
                var result = OnKeyDown.Invoke(str.ToString());
                if (result)
                {
                    e.Handled = true;
                }
            }
        }

        public void Dispose()
        {
            if (hook != null) // Hook will be null if auto update was successful
            {
                hook.KeyDown -= Hook_KeyDown;
                hook.Dispose();
            }
        }

        public void Copy()
        {
            SendKeys.SendWait("^{c}");
        }

        public void SendCommand(KeyboardCommandEnum command)
        {
            switch (command)
            {
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
                    if (string.IsNullOrEmpty(configuration.Character_Name))
                    {
                        logger.Log(@"This command requires a ""CharacterName"" to be specified in the settings menu.", LogState.Warning);
                        return;
                    }
                    SendKeys.SendWait($"{{Enter}}/kick {configuration.Character_Name}{{Enter}}");
                    break;
                case KeyboardCommandEnum.ReplyToLatestWhisper:
                    SendKeys.SendWait("{Enter}^{a}^{v}");
                    break;
            }
        }

        public void SendInput(string input)
        {
            var sendKeyStr = input
                .Replace("Ctrl+", "^")
                .Replace("Space", " ");
            sendKeyStr = Regex.Replace(sendKeyStr, "([a-zA-Z])", "{$1}");
            SendKeys.SendWait(sendKeyStr);
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
