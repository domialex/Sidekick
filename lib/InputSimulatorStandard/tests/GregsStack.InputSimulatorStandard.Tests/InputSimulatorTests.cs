namespace GregsStack.InputSimulatorStandard.Tests
{
    using System;

    using Moq;

    using Xunit;

    public class InputSimulatorTests
    {
        private IInputSimulator inputSimulator;

        public InputSimulatorTests()
        {
            this.inputSimulator = new InputSimulator(Mock.Of<IKeyboardSimulator>(), Mock.Of<IMouseSimulator>(), Mock.Of<IInputDeviceStateAdapter>());
        }

        public class Constructor : InputSimulatorTests
        {
            [Fact]
            public void NullKeyboardSimulatorThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>("keyboardSimulator", () =>
                {
                    this.inputSimulator = new InputSimulator(null, Mock.Of<IMouseSimulator>(), Mock.Of<IInputDeviceStateAdapter>());
                });
            }

            [Fact]
            public void NullMouseSimulatorThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>("mouseSimulator", () =>
                {
                    this.inputSimulator = new InputSimulator(Mock.Of<IKeyboardSimulator>(), null, Mock.Of<IInputDeviceStateAdapter>());
                });
            }

            [Fact]
            public void NullInputDeviceStateAdapterThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>("inputDeviceStateAdapter", () =>
                {
                    this.inputSimulator = new InputSimulator(Mock.Of<IKeyboardSimulator>(), Mock.Of<IMouseSimulator>(), null);
                });
            }
        }

        public class KeyboardProperty : InputSimulatorTests
        {
            [Fact]
            public void IsNotNull()
            {
                var result = this.inputSimulator.Keyboard;
                Assert.IsAssignableFrom<IKeyboardSimulator>(result);
            }
        }

        public class MouseProperty : InputSimulatorTests
        {
            [Fact]
            public void IsNotNull()
            {
                var result = this.inputSimulator.Mouse;
                Assert.IsAssignableFrom<IMouseSimulator>(result);
            }
        }

        public class InputDeviceStateProperty : InputSimulatorTests
        {
            [Fact]
            public void IsNotNull()
            {
                var result = this.inputSimulator.InputDeviceState;
                Assert.IsAssignableFrom<IInputDeviceStateAdapter>(result);
            }
        }
    }
}
