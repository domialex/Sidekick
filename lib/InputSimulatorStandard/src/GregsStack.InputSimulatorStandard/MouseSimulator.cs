namespace GregsStack.InputSimulatorStandard
{
    using System;
    using System.Threading;

    using Native;

    /// <inheritdoc />
    /// <summary>
    /// Implements the <see cref="IMouseSimulator" /> interface by calling the an <see cref="IInputMessageDispatcher" /> to simulate Mouse gestures.
    /// </summary>
    public class MouseSimulator : IMouseSimulator
    {
        /// <summary>
        /// The instance of the <see cref="IInputMessageDispatcher"/> to use for dispatching <see cref="Input"/> messages.
        /// </summary>
        private readonly IInputMessageDispatcher messageDispatcher;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:InputSimulatorStandard.MouseSimulator" /> class using an instance of a <see cref="T:InputSimulatorStandard.WindowsInputMessageDispatcher" /> for dispatching <see cref="T:InputSimulatorStandard.Native.Input" /> messages.
        /// </summary>
        public MouseSimulator()
            : this(new WindowsInputMessageDispatcher())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MouseSimulator"/> class using the specified <see cref="IInputMessageDispatcher"/> for dispatching <see cref="Input"/> messages.
        /// </summary>
        /// <param name="messageDispatcher">The <see cref="IInputMessageDispatcher"/> to use for dispatching <see cref="Input"/> messages.</param>
        /// <exception cref="InvalidOperationException">If null is passed as the <paramref name="messageDispatcher"/>.</exception>
        internal MouseSimulator(IInputMessageDispatcher messageDispatcher)
        {
            this.messageDispatcher = messageDispatcher ?? throw new InvalidOperationException(
                                         string.Format("The {0} cannot operate with a null {1}. Please provide a valid {1} instance to use for dispatching {2} messages.",
                                             typeof(MouseSimulator).Name, typeof(IInputMessageDispatcher).Name, typeof(Input).Name));
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets or sets the amount of mouse wheel scrolling per click. The default value for this property is 120 and different values may cause some applications to interpret the scrolling differently than expected.
        /// </summary>
        public int MouseWheelClickSize { get; set; } = 120;

        /// <inheritdoc />
        /// <summary>
        /// Retrieves the position of the mouse cursor, in screen coordinates.
        /// </summary>
        public System.Drawing.Point Position
        {
            get
            {
                NativeMethods.GetCursorPos(out Point lpPoint);
                return lpPoint;
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates mouse movement by the specified distance measured as a delta from the current mouse location in pixels.
        /// </summary>
        /// <param name="pixelDeltaX">The distance in pixels to move the mouse horizontally.</param>
        /// <param name="pixelDeltaY">The distance in pixels to move the mouse vertically.</param>
        public IMouseSimulator MoveMouseBy(int pixelDeltaX, int pixelDeltaY)
        {
            var inputList = new InputBuilder().AddRelativeMouseMovement(pixelDeltaX, pixelDeltaY).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates mouse movement to the specified location on the primary display device.
        /// </summary>
        /// <param name="absoluteX">The destination's absolute X-coordinate on the primary display device where 0 is the extreme left hand side of the display device and 65535 is the extreme right hand side of the display device.</param>
        /// <param name="absoluteY">The destination's absolute Y-coordinate on the primary display device where 0 is the top of the display device and 65535 is the bottom of the display device.</param>
        public IMouseSimulator MoveMouseTo(double absoluteX, double absoluteY)
        {
            var inputList = new InputBuilder().AddAbsoluteMouseMovement((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates mouse movement to the specified location on the Virtual Desktop which includes all active displays.
        /// </summary>
        /// <param name="absoluteX">The destination's absolute X-coordinate on the virtual desktop where 0 is the left hand side of the virtual desktop and 65535 is the extreme right hand side of the virtual desktop.</param>
        /// <param name="absoluteY">The destination's absolute Y-coordinate on the virtual desktop where 0 is the top of the virtual desktop and 65535 is the bottom of the virtual desktop.</param>
        public IMouseSimulator MoveMouseToPositionOnVirtualDesktop(double absoluteX, double absoluteY)
        {
            var inputList = new InputBuilder().AddAbsoluteMouseMovementOnVirtualDesktop((int)Math.Truncate(absoluteX), (int)Math.Truncate(absoluteY)).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse left button down gesture.
        /// </summary>
        public IMouseSimulator LeftButtonDown() => this.ButtonDown(MouseButton.LeftButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse left button up gesture.
        /// </summary>
        public IMouseSimulator LeftButtonUp() => this.ButtonUp(MouseButton.LeftButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse left-click gesture.
        /// </summary>
        public IMouseSimulator LeftButtonClick() => this.ButtonClick(MouseButton.LeftButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse left button double-click gesture.
        /// </summary>
        public IMouseSimulator LeftButtonDoubleClick() => this.ButtonDoubleClick(MouseButton.LeftButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse middle button down gesture.
        /// </summary>
        public IMouseSimulator MiddleButtonDown() => this.ButtonDown(MouseButton.MiddleButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse middle button up gesture.
        /// </summary>
        public IMouseSimulator MiddleButtonUp() => this.ButtonUp(MouseButton.MiddleButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse middle button click gesture.
        /// </summary>
        public IMouseSimulator MiddleButtonClick() => this.ButtonClick(MouseButton.MiddleButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse middle button double click gesture.
        /// </summary>
        public IMouseSimulator MiddleButtonDoubleClick() => this.ButtonDoubleClick(MouseButton.MiddleButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse right button down gesture.
        /// </summary>
        public IMouseSimulator RightButtonDown() => this.ButtonDown(MouseButton.RightButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse right button up gesture.
        /// </summary>
        public IMouseSimulator RightButtonUp() => this.ButtonUp(MouseButton.RightButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse right button click gesture.
        /// </summary>
        public IMouseSimulator RightButtonClick() => this.ButtonClick(MouseButton.RightButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse right button double-click gesture.
        /// </summary>
        public IMouseSimulator RightButtonDoubleClick() => this.ButtonDoubleClick(MouseButton.RightButton);

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse X button down gesture.
        /// </summary>
        /// <param name="buttonId">The button id.</param>
        public IMouseSimulator XButtonDown(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonDown(buttonId).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse X button up gesture.
        /// </summary>
        /// <param name="buttonId">The button id.</param>
        public IMouseSimulator XButtonUp(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonUp(buttonId).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse X button click gesture.
        /// </summary>
        /// <param name="buttonId">The button id.</param>
        public IMouseSimulator XButtonClick(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonClick(buttonId).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse X button double-click gesture.
        /// </summary>
        /// <param name="buttonId">The button id.</param>
        public IMouseSimulator XButtonDoubleClick(int buttonId)
        {
            var inputList = new InputBuilder().AddMouseXButtonDoubleClick(buttonId).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates mouse vertical wheel scroll gesture.
        /// </summary>
        /// <param name="scrollAmountInClicks">The amount to scroll in clicks. A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that the wheel was rotated backward, toward the user.</param>
        public IMouseSimulator VerticalScroll(int scrollAmountInClicks)
        {
            var inputList = new InputBuilder().AddMouseVerticalWheelScroll(scrollAmountInClicks * this.MouseWheelClickSize).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Simulates a mouse horizontal wheel scroll gesture. Supported by Windows Vista and later.
        /// </summary>
        /// <param name="scrollAmountInClicks">The amount to scroll in clicks. A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was rotated to the left.</param>
        public IMouseSimulator HorizontalScroll(int scrollAmountInClicks)
        {
            var inputList = new InputBuilder().AddMouseHorizontalWheelScroll(scrollAmountInClicks * this.MouseWheelClickSize).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        /// <inheritdoc />
        /// <summary>
        /// Sleeps the executing thread to create a pause between simulated inputs.
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait.</param>
        public IMouseSimulator Sleep(int millisecondsTimeout)
        {
            return this.Sleep(TimeSpan.FromMilliseconds(millisecondsTimeout));
        }

        /// <inheritdoc />
        /// <summary>
        /// Sleeps the executing thread to create a pause between simulated inputs.
        /// </summary>
        /// <param name="timeout">The time to wait.</param>
        public IMouseSimulator Sleep(TimeSpan timeout)
        {
            Thread.Sleep(timeout);
            return this;
        }

        /// <summary>
        /// Sends the list of <see cref="Input"/> messages using the <see cref="IInputMessageDispatcher"/> instance.
        /// </summary>
        /// <param name="inputList">The <see cref="System.Array"/> of <see cref="Input"/> messages to send.</param>
        private void SendSimulatedInput(Input[] inputList)
        {
            this.messageDispatcher.DispatchInput(inputList);
        }

        private IMouseSimulator ButtonDown(MouseButton mouseButton)
        {
            var inputList = new InputBuilder().AddMouseButtonDown(mouseButton).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        private IMouseSimulator ButtonUp(MouseButton mouseButton)
        {
            var inputList = new InputBuilder().AddMouseButtonUp(mouseButton).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        private IMouseSimulator ButtonClick(MouseButton mouseButton)
        {
            var inputList = new InputBuilder().AddMouseButtonClick(mouseButton).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }

        private IMouseSimulator ButtonDoubleClick(MouseButton mouseButton)
        {
            var inputList = new InputBuilder().AddMouseButtonDoubleClick(mouseButton).ToArray();
            this.SendSimulatedInput(inputList);
            return this;
        }
    }
}
