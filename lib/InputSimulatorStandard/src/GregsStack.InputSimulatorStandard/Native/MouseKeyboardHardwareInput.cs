namespace GregsStack.InputSimulatorStandard.Native
{
    using System.Runtime.InteropServices;

    /// <summary>
    /// The combined structure that includes Mouse, Keyboard and Hardware Input message data (see: http://msdn.microsoft.com/en-us/library/ms646270(VS.85).aspx)
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal struct MouseKeyboardHardwareInput
    {
        /// <summary>
        /// The <see cref="MouseInput"/> definition.
        /// </summary>
        [FieldOffset(0)]
        public MouseInput Mouse;

        /// <summary>
        /// The <see cref="KeyboardInput"/> definition.
        /// </summary>
        [FieldOffset(0)]
        public KeyboardInput Keyboard;

        /// <summary>
        /// The <see cref="HardwareInput"/> definition.
        /// </summary>
        [FieldOffset(0)]
        public HardwareInput Hardware;
    }
}
