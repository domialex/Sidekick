namespace GregsStack.InputSimulatorStandard.Tests
{
    using System;

    using Native;

    using Xunit;

    public class WindowsInputMessageDispatcherTests
    {
        private readonly IInputMessageDispatcher inputMessageDispatcher;

        public WindowsInputMessageDispatcherTests()
        {
            this.inputMessageDispatcher = new WindowsInputMessageDispatcher();
        }

        public class DispatchInputMethod : WindowsInputMessageDispatcherTests
        {
            [Fact]
            public void NullInputsThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>("inputs", () => this.inputMessageDispatcher.DispatchInput(null));
            }

            [Fact]
            public void EmptyInputsThrowsArgumentException()
            {
                Assert.Throws<ArgumentException>("inputs", () => this.inputMessageDispatcher.DispatchInput(new Input[0]));
            }
        }
    }
}
