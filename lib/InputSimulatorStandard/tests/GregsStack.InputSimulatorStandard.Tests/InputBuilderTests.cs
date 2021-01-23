namespace GregsStack.InputSimulatorStandard.Tests
{
    using System.Collections.Generic;

    using Native;

    using Xunit;

    public class InputBuilderTests
    {
        private readonly InputBuilder inputBuilder;

        public InputBuilderTests()
        {
            this.inputBuilder = new InputBuilder();
        }

        public class GetEnumeratorMethod : InputBuilderTests
        {
            [Fact]
            public void ReturnsIEnumerator()
            {
                var result = this.inputBuilder.GetEnumerator();
                Assert.IsAssignableFrom<IEnumerator<Input>>(result);
            }
        }

        public class IsExtendedKeyMethod : InputBuilderTests
        {
            [Theory]
            [InlineData(VirtualKeyCode.NUMPAD_RETURN)]
            [InlineData(VirtualKeyCode.MENU)]
            [InlineData(VirtualKeyCode.RMENU)]
            [InlineData(VirtualKeyCode.CONTROL)]
            [InlineData(VirtualKeyCode.RCONTROL)]
            [InlineData(VirtualKeyCode.INSERT)]
            [InlineData(VirtualKeyCode.DELETE)]
            [InlineData(VirtualKeyCode.HOME)]
            [InlineData(VirtualKeyCode.END)]
            [InlineData(VirtualKeyCode.PRIOR)]
            [InlineData(VirtualKeyCode.NEXT)]
            [InlineData(VirtualKeyCode.RIGHT)]
            [InlineData(VirtualKeyCode.UP)]
            [InlineData(VirtualKeyCode.LEFT)]
            [InlineData(VirtualKeyCode.DOWN)]
            [InlineData(VirtualKeyCode.NUMLOCK)]
            [InlineData(VirtualKeyCode.CANCEL)]
            [InlineData(VirtualKeyCode.SNAPSHOT)]
            [InlineData(VirtualKeyCode.DIVIDE)]
            public void ReturnsTrue(VirtualKeyCode keyCode)
            {
                var result = InputBuilder.IsExtendedKey(keyCode);
                Assert.True(result);
            }
        }

        public class AddKeyDownMethod : InputBuilderTests
        {
            [Fact]
            public void AddKeyDown()
            {
                Assert.Empty(this.inputBuilder.ToArray());
                this.inputBuilder.AddKeyDown(VirtualKeyCode.VK_A);
                Assert.Single(this.inputBuilder);
                Assert.Equal((uint)InputType.Keyboard, this.inputBuilder[0].Type);
                Assert.Equal((ushort)VirtualKeyCode.VK_A, this.inputBuilder[0].Data.Keyboard.KeyCode);
            }
        }
    }
}
