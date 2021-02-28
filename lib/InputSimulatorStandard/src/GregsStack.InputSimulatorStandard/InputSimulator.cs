namespace GregsStack.InputSimulatorStandard
{
    using System;

    /// <inheritdoc />
    /// <summary>
    /// Implements the <see cref="IInputSimulator" /> interface to simulate Keyboard and Mouse input and provide the state of those input devices.
    /// </summary>
    public class InputSimulator : IInputSimulator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InputSimulator"/> class using the default <see cref="KeyboardSimulator"/>, <see cref="MouseSimulator"/> and <see cref="WindowsInputDeviceStateAdapter"/> instances.
        /// </summary>
        public InputSimulator()
            : this(new KeyboardSimulator(), new MouseSimulator(), new WindowsInputDeviceStateAdapter())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InputSimulator"/> class using the specified <see cref="IKeyboardSimulator"/>, <see cref="IMouseSimulator"/> and <see cref="IInputDeviceStateAdapter"/> instances.
        /// </summary>
        /// <param name="keyboardSimulator">The <see cref="IKeyboardSimulator"/> instance to use for simulating keyboard input.</param>
        /// <param name="mouseSimulator">The <see cref="IMouseSimulator"/> instance to use for simulating mouse input.</param>
        /// <param name="inputDeviceStateAdapter">The <see cref="IInputDeviceStateAdapter"/> instance to use for interpreting the state of input devices.</param>
        public InputSimulator(IKeyboardSimulator keyboardSimulator, IMouseSimulator mouseSimulator, IInputDeviceStateAdapter inputDeviceStateAdapter)
        {
            this.Keyboard = keyboardSimulator ?? throw new ArgumentNullException(nameof(keyboardSimulator));
            this.Mouse = mouseSimulator ?? throw new ArgumentNullException(nameof(mouseSimulator));
            this.InputDeviceState = inputDeviceStateAdapter ?? throw new ArgumentNullException(nameof(inputDeviceStateAdapter));
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the <see cref="IKeyboardSimulator" /> instance for simulating Keyboard input.
        /// </summary>
        /// <value>The <see cref="IKeyboardSimulator" /> instance.</value>
        public IKeyboardSimulator Keyboard { get; }

        /// <inheritdoc />
        /// <summary>
        /// Gets the <see cref="IMouseSimulator" /> instance for simulating Mouse input.
        /// </summary>
        /// <value>The <see cref="IMouseSimulator" /> instance.</value>
        public IMouseSimulator Mouse { get; }

        /// <inheritdoc />
        /// <summary>
        /// Gets the <see cref="IInputDeviceStateAdapter" /> instance for determining the state of the various input devices.
        /// </summary>
        /// <value>The <see cref="IInputDeviceStateAdapter" /> instance.</value>
        public IInputDeviceStateAdapter InputDeviceState { get; }
    }
}
