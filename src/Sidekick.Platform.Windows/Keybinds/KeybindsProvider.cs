using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GregsStack.InputSimulatorStandard;
using GregsStack.InputSimulatorStandard.Native;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Platforms;
using WindowsHook;

namespace Sidekick.Platform.Windows.Keybinds
{
    public class KeybindsProvider : IKeyboardProvider, IDisposable
    {
        private readonly ILogger<KeybindsProvider> logger;

        private static readonly List<Keys> KEYS_INVALID = new List<Keys>() {
            Keys.ControlKey,
            Keys.LControlKey,
            Keys.RControlKey,
            Keys.ShiftKey,
            Keys.RShiftKey,
            Keys.LShiftKey,
            Keys.RWin,
            Keys.LWin,
            Keys.LMenu,
            Keys.RMenu,
        };

        private static InputSimulator Simulator { get; set; }

        public KeybindsProvider(ILogger<KeybindsProvider> logger)
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
            if (KEYS_INVALID.Contains(e.KeyCode))
            {
                return;
            }

            // Transfer the event key to a string to compare to settings
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
            if (e.Modifiers.HasFlag(Keys.LWin) || e.Modifiers.HasFlag(Keys.RWin))
            {
                str.Append("Win+");
            }

            str.Append(e.KeyCode);

            str = str
                .Replace("Back", "Backspace")
                .Replace("Capital", "CapsLock")
                .Replace("Next", "PageDown")
                .Replace("Pause", "Break")
                .Replace("Return", "Enter");

            e.Handled = OnKeyDown?.Invoke(str.ToString()) ?? false;
        }

        public bool IsCtrlPressed()
        {
            return Simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.CONTROL)
                || Simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.LCONTROL)
                || Simulator.InputDeviceState.IsKeyDown(VirtualKeyCode.RCONTROL);
        }

        public Task PressKey(params string[] strokes)
        {
            var sendKeyStr = "";

            foreach (var stroke in strokes)
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
                    switch (key)
                    {
                        case "Shift": modifiedCodes.Add(VirtualKeyCode.SHIFT); break;
                        case "Ctrl": modifiedCodes.Add(VirtualKeyCode.CONTROL); break;
                        case "Alt": modifiedCodes.Add(VirtualKeyCode.MENU); break;
                        case "Up": keyCodes.Add(VirtualKeyCode.UP); break;
                        case "Down": keyCodes.Add(VirtualKeyCode.DOWN); break;
                        case "Right": keyCodes.Add(VirtualKeyCode.RIGHT); break;
                        case "Left": keyCodes.Add(VirtualKeyCode.LEFT); break;
                        case "Backspace": keyCodes.Add(VirtualKeyCode.BACK); break;
                        case "Break": keyCodes.Add(VirtualKeyCode.CANCEL); break;
                        case "CapsLock": keyCodes.Add(VirtualKeyCode.CAPITAL); break;
                        case "Delete": keyCodes.Add(VirtualKeyCode.DELETE); break;
                        case "End": keyCodes.Add(VirtualKeyCode.END); break;
                        case "Enter": keyCodes.Add(VirtualKeyCode.RETURN); break;
                        case "Esc": keyCodes.Add(VirtualKeyCode.ESCAPE); break;
                        case "Help": keyCodes.Add(VirtualKeyCode.HELP); break;
                        case "Home": keyCodes.Add(VirtualKeyCode.HOME); break;
                        case "Insert": keyCodes.Add(VirtualKeyCode.INSERT); break;
                        case "NumLock": keyCodes.Add(VirtualKeyCode.NUMLOCK); break;
                        case "PageDown": keyCodes.Add(VirtualKeyCode.NEXT); break;
                        case "PageUp": keyCodes.Add(VirtualKeyCode.PRIOR); break;
                        case "PrintScreen": keyCodes.Add(VirtualKeyCode.PRINT); break;
                        case "ScrollLock": keyCodes.Add(VirtualKeyCode.SCROLL); break;
                        case "Space": keyCodes.Add(VirtualKeyCode.SPACE); break;
                        case "Tab": keyCodes.Add(VirtualKeyCode.TAB); break;
                        case "F1": keyCodes.Add(VirtualKeyCode.F1); break;
                        case "F2": keyCodes.Add(VirtualKeyCode.F2); break;
                        case "F3": keyCodes.Add(VirtualKeyCode.F3); break;
                        case "F4": keyCodes.Add(VirtualKeyCode.F4); break;
                        case "F5": keyCodes.Add(VirtualKeyCode.F5); break;
                        case "F6": keyCodes.Add(VirtualKeyCode.F6); break;
                        case "F7": keyCodes.Add(VirtualKeyCode.F7); break;
                        case "F8": keyCodes.Add(VirtualKeyCode.F8); break;
                        case "F9": keyCodes.Add(VirtualKeyCode.F9); break;
                        case "F10": keyCodes.Add(VirtualKeyCode.F10); break;
                        case "F11": keyCodes.Add(VirtualKeyCode.F11); break;
                        case "F12": keyCodes.Add(VirtualKeyCode.F12); break;
                        case "F13": keyCodes.Add(VirtualKeyCode.F13); break;
                        case "F14": keyCodes.Add(VirtualKeyCode.F14); break;
                        case "F15": keyCodes.Add(VirtualKeyCode.F15); break;
                        case "F16": keyCodes.Add(VirtualKeyCode.F16); break;
                        case "Zoom": keyCodes.Add(VirtualKeyCode.ZOOM); break;
                        case "0": keyCodes.Add(VirtualKeyCode.VK_0); break;
                        case "1": keyCodes.Add(VirtualKeyCode.VK_1); break;
                        case "2": keyCodes.Add(VirtualKeyCode.VK_2); break;
                        case "3": keyCodes.Add(VirtualKeyCode.VK_3); break;
                        case "4": keyCodes.Add(VirtualKeyCode.VK_4); break;
                        case "5": keyCodes.Add(VirtualKeyCode.VK_5); break;
                        case "6": keyCodes.Add(VirtualKeyCode.VK_6); break;
                        case "7": keyCodes.Add(VirtualKeyCode.VK_7); break;
                        case "8": keyCodes.Add(VirtualKeyCode.VK_8); break;
                        case "9": keyCodes.Add(VirtualKeyCode.VK_9); break;
                        case "A": keyCodes.Add(VirtualKeyCode.VK_A); break;
                        case "B": keyCodes.Add(VirtualKeyCode.VK_B); break;
                        case "C": keyCodes.Add(VirtualKeyCode.VK_C); break;
                        case "D": keyCodes.Add(VirtualKeyCode.VK_D); break;
                        case "E": keyCodes.Add(VirtualKeyCode.VK_E); break;
                        case "F": keyCodes.Add(VirtualKeyCode.VK_F); break;
                        case "G": keyCodes.Add(VirtualKeyCode.VK_G); break;
                        case "H": keyCodes.Add(VirtualKeyCode.VK_H); break;
                        case "I": keyCodes.Add(VirtualKeyCode.VK_I); break;
                        case "J": keyCodes.Add(VirtualKeyCode.VK_J); break;
                        case "K": keyCodes.Add(VirtualKeyCode.VK_K); break;
                        case "L": keyCodes.Add(VirtualKeyCode.VK_L); break;
                        case "M": keyCodes.Add(VirtualKeyCode.VK_M); break;
                        case "N": keyCodes.Add(VirtualKeyCode.VK_N); break;
                        case "O": keyCodes.Add(VirtualKeyCode.VK_O); break;
                        case "P": keyCodes.Add(VirtualKeyCode.VK_P); break;
                        case "Q": keyCodes.Add(VirtualKeyCode.VK_Q); break;
                        case "R": keyCodes.Add(VirtualKeyCode.VK_R); break;
                        case "S": keyCodes.Add(VirtualKeyCode.VK_S); break;
                        case "T": keyCodes.Add(VirtualKeyCode.VK_T); break;
                        case "U": keyCodes.Add(VirtualKeyCode.VK_U); break;
                        case "V": keyCodes.Add(VirtualKeyCode.VK_V); break;
                        case "W": keyCodes.Add(VirtualKeyCode.VK_W); break;
                        case "X": keyCodes.Add(VirtualKeyCode.VK_X); break;
                        case "Y": keyCodes.Add(VirtualKeyCode.VK_Y); break;
                        case "Z": keyCodes.Add(VirtualKeyCode.VK_Z); break;
                    }
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

            return Task.Delay(100);
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
