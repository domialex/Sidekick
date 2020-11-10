using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Process;
using Sidekick.Domain.Settings;
using WindowsHook;

namespace Sidekick.Keybinds
{
    public class KeybindsProvider : IKeybindsProvider, IDisposable
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
        private readonly INativeProcess nativeProcess;
        private readonly ISidekickSettings settings;

        public KeybindsProvider(
            INativeProcess nativeProcess,
            ISidekickSettings settings)
        {
            this.nativeProcess = nativeProcess;
            this.settings = settings;
        }

        public IKeyboardMouseEvents Hook { get; private set; }

        public event Func<string, bool> OnKeyDown;
        public event Func<bool> OnScrollDown;
        public event Func<bool> OnScrollUp;

        public void Initialize()
        {
            Hook = WindowsHook.Hook.GlobalEvents();
            Hook.KeyDown += Hook_KeyDown;
            Hook.MouseWheelExt += Hook_MouseWheelExt;
        }

        private void Hook_KeyDown(object sender, WindowsHook.KeyEventArgs e)
        {
            if ((!nativeProcess.IsPathOfExileInFocus && !nativeProcess.IsSidekickInFocus) || KEYS_INVALID.Contains(e.KeyCode))
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

            str = str
                .Replace("Back", "Backspace")
                .Replace("Capital", "CapsLock")
                .Replace("Next", "PageDown")
                .Replace("Pause", "Break");

            e.Handled = OnKeyDown?.Invoke(str.ToString()) ?? false;
        }

        private void Hook_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            if (!nativeProcess.IsPathOfExileInFocus || !settings.EnableCtrlScroll || !IsCtrlPressed())
            {
                return;
            }

            if (e.Delta > 0)
            {
                e.Handled = OnScrollUp?.Invoke() ?? false;
            }
            else
            {
                e.Handled = OnScrollDown?.Invoke() ?? false;
            }
        }

        private bool IsCtrlPressed()
        {
            return Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_CONTROL)
                || Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_LCONTROL)
                || Keyboard.IsKeyPressed(Keyboard.VirtualKeyStates.VK_RCONTROL);
        }

        private static readonly Regex SendKeyReplace = new Regex("([a-zA-Z]+(?![^{]*\\}))");
        public void PressKey(string keys)
        {
            var sendKeyStr = keys
                .Replace("Shift+", "+")
                .Replace("Ctrl+", "^")
                .Replace("Alt+", "%")
                .Replace("Up", "{Up}")
                .Replace("Down", "{Down}")
                .Replace("Right", "{Right}")
                .Replace("Left", "{Left}")
                .Replace("Backspace", "{Backspace}")
                .Replace("Break", "{Break}")
                .Replace("CapsLock", "{CapsLock}")
                .Replace("Delete", "{Delete}")
                .Replace("End", "{End}")
                .Replace("Enter", "{Enter}")
                .Replace("Esc", "{Esc}")
                .Replace("Help", "{Help}")
                .Replace("Home", "{Home}")
                .Replace("Insert", "{Insert}")
                .Replace("NumLock", "{NumLock}")
                .Replace("PageDown", "{Pgdn}")
                .Replace("PageUp", "{Pgup}")
                .Replace("PrintScreen", "{PrtSc}")
                .Replace("ScrollLock", "{ScrollLock}")
                .Replace("Space", "{Space}")
                .Replace("ScrollLock", "{ScrollLock}")
                .Replace("Tab", "{Tab}")
                .Replace("F1", "{F1}")
                .Replace("F2", "{F2}")
                .Replace("F3", "{F3}")
                .Replace("F4", "{F4}")
                .Replace("F5", "{F5}")
                .Replace("F6", "{F6}")
                .Replace("F7", "{F7}")
                .Replace("F8", "{F8}")
                .Replace("F9", "{F9}")
                .Replace("F10", "{F10}")
                .Replace("F11", "{F11}")
                .Replace("F12", "{F12}")
                .Replace("F13", "{F13}")
                .Replace("F14", "{F14}")
                .Replace("F15", "{F15}")
                .Replace("F16", "{F16}")
                .Replace("Copy", "^{c}")
                .Replace("Paste", "^{v}");
            sendKeyStr = SendKeyReplace.Replace(sendKeyStr, "{$1}");
            SendKeys.SendWait(sendKeyStr);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Hook != null) // Hook will be null if auto update was successful
            {
                Hook.KeyDown -= Hook_KeyDown;
                Hook.MouseWheelExt -= Hook_MouseWheelExt;
                Hook.Dispose();
            }
        }
    }
}
