using GregsStack.InputSimulatorStandard.Native;
using NeatInput.Windows.Processing.Keyboard.Enums;

namespace Sidekick.Platform.Windows.Keyboards
{
    public struct Key
    {
        public Key(Keys hookKey, VirtualKeyCode sendKey, string stringValue, string electronAccelerator)
        {
            HookKey = hookKey;
            SendKey = sendKey;
            StringValue = stringValue;
            ElectronAccelerator = electronAccelerator;
        }

        public VirtualKeyCode SendKey { get; }

        public Keys HookKey { get; }

        public string StringValue { get; }

        public string ElectronAccelerator { get; }
    }
}
