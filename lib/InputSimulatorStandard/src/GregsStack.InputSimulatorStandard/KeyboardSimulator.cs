namespace GregsStack.InputSimulatorStandard
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Native;

    /// <inheritdoc />
    /// <summary>
    /// Implements the <see cref="IKeyboardSimulator" /> interface by calling the an <see cref="IInputMessageDispatcher" /> to simulate Keyboard gestures.
    /// </summary>
    public class KeyboardSimulator : IKeyboardSimulator
    {
        /// <summary>
        /// The instance of the <see cref="IInputMessageDispatcher"/> to use for dispatching <see cref="Input"/> messages.
        /// </summary>
        private readonly IInputMessageDispatcher messageDispatcher;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:InputSimulatorStandard.KeyboardSimulator" /> class using an instance of a <see cref="T:InputSimulatorStandard.WindowsInputMessageDispatcher" /> for dispatching <see cref="T:InputSimulatorStandard.Native.Input" /> messages.
        /// </summary>
        public KeyboardSimulator()
            : this(new WindowsInputMessageDispatcher())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyboardSimulator"/> class using the specified <see cref="IInputMessageDispatcher"/> for dispatching <see cref="Input"/> messages.
        /// </summary>
        /// <param name="messageDispatcher">The <see cref="IInputMessageDispatcher"/> to use for dispatching <see cref="Input"/> messages.</param>
        /// <exception cref="InvalidOperationException">If null is passed as the <paramref name="messageDispatcher"/>.</exception>
        internal KeyboardSimulator(IInputMessageDispatcher messageDispatcher)
        {
            this.messageDispatcher = messageDispatcher ?? throw new InvalidOperationException(
                                         string.Format("The {0} cannot operate with a null {1}. Please provide a valid {1} instance to use for dispatching {2} messages.",
                                             typeof(KeyboardSimulator).Name, typeof(IInputMessageDispatcher).Name, typeof(Input).Name));
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the Win32 SendInput method to simulate a KeyDown.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode" /> to press</param>
        public IKeyboardSimulator KeyDown(VirtualKeyCode keyCode)
        {
            var inputList = new InputBuilder().AddKeyDown(keyCode).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the Win32 SendInput method to simulate a KeyUp.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode" /> to lift up</param>
        public IKeyboardSimulator KeyUp(VirtualKeyCode keyCode)
        {
            var inputList = new InputBuilder().AddKeyUp(keyCode).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the Win32 SendInput method with a KeyDown and KeyUp message in the same input sequence in order to simulate a Key PRESS.
        /// </summary>
        /// <param name="keyCode">The <see cref="VirtualKeyCode" /> to press</param>
        public IKeyboardSimulator KeyPress(VirtualKeyCode keyCode)
        {
            var inputList = new InputBuilder().AddKeyPress(keyCode).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a key press for each of the specified key codes in the order they are specified.
        /// </summary>
        /// <param name="keyCodes"></param>
        public IKeyboardSimulator KeyPress(params VirtualKeyCode[] keyCodes)
        {
            var builder = new InputBuilder();
            this.KeysPress(builder, keyCodes);
            this.SendSimulatedInput(builder.ToArray());
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a simple modified keystroke like CTRL-C where CTRL is the modifierKey and C is the key.
        /// The flow is Modifier KeyDown, Key Press, Modifier KeyUp.
        /// </summary>
        /// <param name="modifierKeyCode">The modifier key</param>
        /// <param name="keyCode">The key to simulate</param>
        public IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
        {
            this.ModifiedKeyStroke(new[] { modifierKeyCode }, new[] { keyCode });
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a modified keystroke where there are multiple modifiers and one key like CTRL-ALT-C where CTRL and ALT are the modifierKeys and C is the key.
        /// The flow is Modifiers KeyDown in order, Key Press, Modifiers KeyUp in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCode">The key to simulate</param>
        public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
        {
            this.ModifiedKeyStroke(modifierKeyCodes, new[] { keyCode });
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the modifierKey and K and C are the keys.
        /// The flow is Modifier KeyDown, Keys Press in order, Modifier KeyUp.
        /// </summary>
        /// <param name="modifierKey">The modifier key</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public IKeyboardSimulator ModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
        {
            this.ModifiedKeyStroke(new[] { modifierKey }, keyCodes);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL and ALT are the modifierKeys and K and C are the keys.
        /// The flow is Modifiers KeyDown in order, Keys Press in order, Modifiers KeyUp in reverse order.
        /// </summary>
        /// <param name="modifierKeyCodes">The list of modifier keys</param>
        /// <param name="keyCodes">The list of keys to simulate</param>
        public IKeyboardSimulator ModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
        {
            var builder = new InputBuilder();
            this.ModifiersDown(builder, modifierKeyCodes);
            this.KeysPress(builder, keyCodes);
            this.ModifiersUp(builder, modifierKeyCodes);

            this.SendSimulatedInput(builder.ToArray());
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
        /// </summary>
        /// <param name="text">The text to be simulated.</param>
        public IKeyboardSimulator TextEntry(string text)
        {
            if (text.Length > uint.MaxValue / 2)
            {
                throw new ArgumentException($"The text parameter is too long. It must be less than {uint.MaxValue / 2} characters.", nameof(text));
            }

            var inputList = new InputBuilder().AddCharacters(text).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a single character text entry via the keyboard.
        /// </summary>
        /// <param name="character">The unicode character to be simulated.</param>
        public IKeyboardSimulator TextEntry(char character)
        {
            var inputList = new InputBuilder().AddCharacter(character).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sleeps the executing thread to create a pause between simulated inputs.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait.</param>
        public IKeyboardSimulator Sleep(int millisecondsTimeout)
        {
            return this.Sleep(TimeSpan.FromMilliseconds(millisecondsTimeout));
        }

        /// <inheritdoc />
        /// <summary>
        /// Sleeps the executing thread to create a pause between simulated inputs.
        /// </summary>
        /// <param name="timeout">The time to wait.</param>
        public IKeyboardSimulator Sleep(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
            return this;
        }

        private void ModifiersDown(InputBuilder builder, IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            if (modifierKeyCodes == null)
            {
                return;
            }

            foreach (var key in modifierKeyCodes)
            {
                builder.AddKeyDown(key);
            }
        }

        private void ModifiersUp(InputBuilder builder, IEnumerable<VirtualKeyCode> modifierKeyCodes)
        {
            if (modifierKeyCodes == null)
            {
                return;
            }

            // Key up in reverse (I miss LINQ)
            var stack = new Stack<VirtualKeyCode>(modifierKeyCodes);
            while (stack.Count > 0)
            {
                builder.AddKeyUp(stack.Pop());
            }
        }

        private void KeysPress(InputBuilder builder, IEnumerable<VirtualKeyCode> keyCodes)
        {
            if (keyCodes == null)
            {
                return;
            }

            foreach (var key in keyCodes)
            {
                builder.AddKeyPress(key);
            }
        }

        /// <summary>
        /// Sends the list of <see cref="Input"/> messages using the <see cref="IInputMessageDispatcher"/> instance.
        /// </summary>
        /// <param name="inputList">The <see cref="Array"/> of <see cref="Input"/> messages to send.</param>
        private void SendSimulatedInput(Input[] inputList)
        {
            this.messageDispatcher.DispatchInput(inputList);
        }
    }
}
