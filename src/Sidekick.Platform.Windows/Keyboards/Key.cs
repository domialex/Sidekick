using GregsStack.InputSimulatorStandard.Native;
using NeatInput.Windows.Processing.Keyboard.Enums;

namespace Sidekick.Platform.Windows.Keyboards
{
    public struct Key
    {
        public Key(Keys hookKey, VirtualKeyCode sendKey, string stringValue)
        {
            HookKey = hookKey;
            SendKey = sendKey;
            StringValue = stringValue;
        }

        public VirtualKeyCode SendKey { get; set; }

        public Keys HookKey { get; set; }

        public string StringValue { get; set; }
    }
}
