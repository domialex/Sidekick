using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Serilog;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Natives.Helpers;
using WindowsHook;

namespace Sidekick.Natives
{
    public class NativeKeyboard : INativeKeyboard, IOnAfterInit, IDisposable
    {
        private static readonly List<WindowsHook.Keys> KEYS_INVALID = new List<WindowsHook.Keys>() {
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

        public NativeKeyboard(ILogger logger)
        {
            this.logger = logger.ForContext(GetType());
        }

        public bool Enabled { get; set; }

        public event Func<string, bool> OnKeyDown;

        private IKeyboardMouseEvents hook = null;
        private bool isDisposed;

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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                if (hook != null) // Hook will be null if auto update was successful
                {
                    hook.KeyDown -= Hook_KeyDown;
                    hook.Dispose();
                }
            }

            isDisposed = true;
        }

        public void Copy()
        {
            SendKeys.SendWait("^{c}");
        }

        public void Paste()
        {
            SendKeys.SendWait("^{v}");
        }

        public void SendInput(string input)
        {
            var sendKeyStr = input
                .Replace("Ctrl+", "^")
                .Replace("Space", " ")
                .Replace("Enter", "{Enter}")
                .Replace("Up", "{Up}")
                .Replace("Down", "{Down}")
                .Replace("Right", "{Right}")
                .Replace("Left", "{Left}")
                .Replace("Esc", "{Esc}");
            sendKeyStr = Regex.Replace(sendKeyStr, "([a-zA-Z]+(?![^{]*\\}))", "{$1}");
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
                    logger.Warning("NativeKeyboard.IsKeyPressed - Unrecognized key - {key}", key);
                    return false;
            };
        }
    }
}
