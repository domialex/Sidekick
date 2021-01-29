using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Platforms;
using WindowsHook;

namespace Sidekick.Platform.Windows.Keyboards
{
    public class KeyboardProvider : IKeyboardProvider, IDisposable
    {
        private static readonly List<Key> ValidKeys = new List<Key>()
        {
            new Key(Keys.ShiftKey, VirtualKeyCode.SHIFT, "Shift"),
            new Key(Keys.LShiftKey, VirtualKeyCode.LSHIFT, "Shift"),
            new Key(Keys.RShiftKey, VirtualKeyCode.RSHIFT, "Shift"),
            new Key(Keys.ControlKey, VirtualKeyCode.CONTROL, "Ctrl"),
            new Key(Keys.LControlKey, VirtualKeyCode.LCONTROL, "Ctrl"),
            new Key(Keys.RControlKey, VirtualKeyCode.RCONTROL, "Ctrl"),
            new Key(Keys.Menu, VirtualKeyCode.MENU, "Alt"),
            new Key(Keys.LMenu, VirtualKeyCode.LMENU, "Alt"),
            new Key(Keys.RMenu, VirtualKeyCode.RMENU, "Alt"),

            new Key(Keys.F1, VirtualKeyCode.F1, "F1"),
            new Key(Keys.F2, VirtualKeyCode.F2, "F2"),
            new Key(Keys.F3, VirtualKeyCode.F3, "F3"),
            new Key(Keys.F4, VirtualKeyCode.F4, "F4"),
            new Key(Keys.F5, VirtualKeyCode.F5, "F5"),
            new Key(Keys.F6, VirtualKeyCode.F6, "F6"),
            new Key(Keys.F7, VirtualKeyCode.F7, "F7"),
            new Key(Keys.F8, VirtualKeyCode.F8, "F8"),
            new Key(Keys.F9, VirtualKeyCode.F9, "F9"),
            new Key(Keys.F10, VirtualKeyCode.F10, "F10"),
            new Key(Keys.F11, VirtualKeyCode.F11, "F11"),
            new Key(Keys.F12, VirtualKeyCode.F12, "F12"),
            new Key(Keys.F13, VirtualKeyCode.F13, "F13"),
            new Key(Keys.F14, VirtualKeyCode.F14, "F14"),
            new Key(Keys.F15, VirtualKeyCode.F15, "F15"),
            new Key(Keys.F16, VirtualKeyCode.F16, "F16"),

            new Key(Keys.D0, VirtualKeyCode.VK_0, "0"),
            new Key(Keys.D1, VirtualKeyCode.VK_1, "1"),
            new Key(Keys.D2, VirtualKeyCode.VK_2, "2"),
            new Key(Keys.D3, VirtualKeyCode.VK_3, "3"),
            new Key(Keys.D4, VirtualKeyCode.VK_4, "4"),
            new Key(Keys.D5, VirtualKeyCode.VK_5, "5"),
            new Key(Keys.D6, VirtualKeyCode.VK_6, "6"),
            new Key(Keys.D7, VirtualKeyCode.VK_7, "7"),
            new Key(Keys.D8, VirtualKeyCode.VK_8, "8"),
            new Key(Keys.D9, VirtualKeyCode.VK_9, "9"),

            new Key(Keys.A, VirtualKeyCode.VK_A, "A"),
            new Key(Keys.B, VirtualKeyCode.VK_B, "B"),
            new Key(Keys.C, VirtualKeyCode.VK_C, "C"),
            new Key(Keys.D, VirtualKeyCode.VK_D, "D"),
            new Key(Keys.E, VirtualKeyCode.VK_E, "E"),
            new Key(Keys.F, VirtualKeyCode.VK_F, "F"),
            new Key(Keys.G, VirtualKeyCode.VK_G, "G"),
            new Key(Keys.H, VirtualKeyCode.VK_H, "H"),
            new Key(Keys.I, VirtualKeyCode.VK_I, "I"),
            new Key(Keys.J, VirtualKeyCode.VK_J, "J"),
            new Key(Keys.K, VirtualKeyCode.VK_K, "K"),
            new Key(Keys.L, VirtualKeyCode.VK_L, "L"),
            new Key(Keys.M, VirtualKeyCode.VK_M, "M"),
            new Key(Keys.N, VirtualKeyCode.VK_N, "N"),
            new Key(Keys.O, VirtualKeyCode.VK_O, "O"),
            new Key(Keys.P, VirtualKeyCode.VK_P, "P"),
            new Key(Keys.Q, VirtualKeyCode.VK_Q, "Q"),
            new Key(Keys.R, VirtualKeyCode.VK_R, "R"),
            new Key(Keys.S, VirtualKeyCode.VK_S, "S"),
            new Key(Keys.T, VirtualKeyCode.VK_T, "T"),
            new Key(Keys.U, VirtualKeyCode.VK_U, "U"),
            new Key(Keys.V, VirtualKeyCode.VK_V, "V"),
            new Key(Keys.W, VirtualKeyCode.VK_W, "W"),
            new Key(Keys.X, VirtualKeyCode.VK_X, "X"),
            new Key(Keys.Y, VirtualKeyCode.VK_Y, "Y"),
            new Key(Keys.Z, VirtualKeyCode.VK_Z, "Z"),

            new Key(Keys.OemMinus, VirtualKeyCode.OEM_MINUS, "-"),
            new Key(Keys.Oemplus, VirtualKeyCode.OEM_PLUS, "="),
            new Key(Keys.Oemcomma, VirtualKeyCode.OEM_COMMA, ","),
            new Key(Keys.OemPeriod, VirtualKeyCode.OEM_PERIOD, ","),
            new Key(Keys.Oem1, VirtualKeyCode.OEM_1, ";"),
            new Key(Keys.OemSemicolon, VirtualKeyCode.OEM_3, "~"),
            new Key(Keys.Oem2, VirtualKeyCode.OEM_2, "/"),
            new Key(Keys.OemQuestion, VirtualKeyCode.OEM_3, "~"),
            new Key(Keys.Oem3, VirtualKeyCode.OEM_3, "~"),
            new Key(Keys.Oemtilde, VirtualKeyCode.OEM_3, "~"),
            new Key(Keys.Oem4, VirtualKeyCode.OEM_4, "["),
            new Key(Keys.OemOpenBrackets, VirtualKeyCode.OEM_4, "["),
            new Key(Keys.Oem5, VirtualKeyCode.OEM_5, "\\"),
            new Key(Keys.OemBackslash, VirtualKeyCode.OEM_5, "\\"),
            new Key(Keys.Oem6, VirtualKeyCode.OEM_6, "]"),
            new Key(Keys.OemCloseBrackets, VirtualKeyCode.OEM_6, "]"),
            new Key(Keys.Oem7, VirtualKeyCode.OEM_7, "'"),
            new Key(Keys.OemQuotes, VirtualKeyCode.OEM_7, "'"),

            new Key(Keys.Escape, VirtualKeyCode.ESCAPE, "Esc"),
            new Key(Keys.Tab, VirtualKeyCode.TAB, "Tab"),
            new Key(Keys.Capital, VirtualKeyCode.CAPITAL, "CapsLock"),
            new Key(Keys.CapsLock, VirtualKeyCode.CAPITAL, "CapsLock"),
            new Key(Keys.Space, VirtualKeyCode.SPACE, "Space"),
            new Key(Keys.Back, VirtualKeyCode.BACK, "Backspace"),
            new Key(Keys.Enter, VirtualKeyCode.RETURN, "Enter"),
            new Key(Keys.Return, VirtualKeyCode.RETURN, "Enter"),
            new Key(Keys.PrintScreen, VirtualKeyCode.PRINT, "PrintScreen"),
            new Key(Keys.Scroll, VirtualKeyCode.SCROLL, "ScrollLock"),
            new Key(Keys.Pause, VirtualKeyCode.PAUSE, "Pause"),
            new Key(Keys.Cancel, VirtualKeyCode.CANCEL, "Break"),
            new Key(Keys.Insert, VirtualKeyCode.INSERT, "Insert"),
            new Key(Keys.Home, VirtualKeyCode.HOME, "Home"),
            new Key(Keys.Delete, VirtualKeyCode.DELETE, "Delete"),
            new Key(Keys.End, VirtualKeyCode.END, "End"),
            new Key(Keys.PageDown, VirtualKeyCode.NEXT, "PageDown"),
            new Key(Keys.PageUp, VirtualKeyCode.PRIOR, "PageUp"),
            new Key(Keys.Help, VirtualKeyCode.HELP, "Help"),
            new Key(Keys.Zoom, VirtualKeyCode.ZOOM, "Zoom"),

            new Key(Keys.Up, VirtualKeyCode.UP, "Up"),
            new Key(Keys.Down, VirtualKeyCode.DOWN, "Down"),
            new Key(Keys.Left, VirtualKeyCode.LEFT, "Left"),
            new Key(Keys.Right, VirtualKeyCode.RIGHT, "Right"),

            new Key(Keys.NumLock, VirtualKeyCode.NUMLOCK, "NumLock"),
            new Key(Keys.NumPad0, VirtualKeyCode.NUMPAD0, "Num0"),
            new Key(Keys.NumPad1, VirtualKeyCode.NUMPAD1, "Num1"),
            new Key(Keys.NumPad2, VirtualKeyCode.NUMPAD2, "Num2"),
            new Key(Keys.NumPad3, VirtualKeyCode.NUMPAD3, "Num3"),
            new Key(Keys.NumPad4, VirtualKeyCode.NUMPAD4, "Num4"),
            new Key(Keys.NumPad5, VirtualKeyCode.NUMPAD5, "Num5"),
            new Key(Keys.NumPad6, VirtualKeyCode.NUMPAD6, "Num6"),
            new Key(Keys.NumPad7, VirtualKeyCode.NUMPAD7, "Num7"),
            new Key(Keys.NumPad8, VirtualKeyCode.NUMPAD8, "Num8"),
            new Key(Keys.NumPad9, VirtualKeyCode.NUMPAD9, "Num9"),
        };

        private static readonly Regex ModifierKeys = new Regex("Ctrl|Shift|Alt");

        private readonly ILogger<KeyboardProvider> logger;

        private static InputSimulator Simulator { get; set; }

        public KeyboardProvider(ILogger<KeyboardProvider> logger)
        {
            this.logger = logger;
        }

        public IKeyboardMouseEvents Hook { get; private set; }

        public event Func<string, bool> OnKeyDown;

        public void Initialize()
        {
            Hook = WindowsHook.Hook.GlobalEvents();
            Hook.KeyDown += Hook_KeyDown;

            Simulator = new InputSimulator();
        }

        private void Hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (!ValidKeys.Any(x => x.HookKey == e.KeyCode))
            {
                return;
            }

            var key = ValidKeys.Find(x => x.HookKey == e.KeyCode);
            if (ModifierKeys.IsMatch(key.StringValue))
            {
                return;
            }

            // Transfer the event key to a string to compare to settings
            var str = new StringBuilder();
            if (e.Modifiers.HasFlag(Keys.Control))
            {
                str.Append($"{ValidKeys.Find(x => x.HookKey == Keys.ControlKey).StringValue }+");
            }
            if (e.Modifiers.HasFlag(Keys.Shift))
            {
                str.Append($"{ValidKeys.Find(x => x.HookKey == Keys.ShiftKey).StringValue }+");
            }
            if (e.Modifiers.HasFlag(Keys.Alt))
            {
                str.Append($"{ValidKeys.Find(x => x.HookKey == Keys.Menu).StringValue }+");
            }

            if (ValidKeys.Any(x => x.HookKey == e.KeyCode))
            {
                str.Append(key.StringValue);
                e.Handled = OnKeyDown?.Invoke(str.ToString()) ?? false;
            }
        }

        public bool IsCtrlPressed() =>
            Simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.CONTROL) ||
            Simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.LCONTROL) ||
            Simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.RCONTROL);

        public Task PressKey(params string[] keys)
        {
            foreach (var stroke in keys)
            {
                logger.LogInformation("[Keybinds] Sending " + stroke);

                switch (stroke)
                {
                    case "Copy":
                        Simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_C);
                        continue;
                    case "Paste":
                        Simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_V);
                        continue;
                }

                var keyCodes = new List<VirtualKeyCode>();
                var modifiedCodes = new List<VirtualKeyCode>();

                foreach (var key in stroke.Split('+'))
                {
                    if (!ValidKeys.Any(x => x.StringValue == key))
                    {
                        continue;
                    }

                    var validKey = ValidKeys.Find(x => x.StringValue == key);
                    if (ModifierKeys.IsMatch(key))
                    {
                        modifiedCodes.Add(validKey.SendKey);
                    }
                    else
                    {
                        keyCodes.Add(validKey.SendKey);
                    }
                }

                if (keyCodes.Count == 0)
                {
                    continue;
                }

                if (modifiedCodes.Count > 0)
                {
                    Simulator.Keyboard.ModifiedKeyStroke(modifiedCodes, keyCodes);
                }
                else
                {
                    Simulator.Keyboard.KeyPress(keyCodes.ToArray());
                }
            }

            return Task.CompletedTask;
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
                Hook.Dispose();
            }
        }
    }
}
