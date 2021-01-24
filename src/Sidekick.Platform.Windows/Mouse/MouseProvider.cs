using System.Drawing;
using GregsStack.InputSimulatorStandard;
using Sidekick.Domain.Platforms;

namespace Sidekick.Platform.Windows.Mouse
{
    public class MouseProvider : IMouseProvider
    {
        private static InputSimulator simulator = null;
        public static InputSimulator Simulator
        {
            get
            {
                if (simulator == null)
                {
                    simulator = new InputSimulator();
                }
                return simulator;
            }
        }

        public MouseProvider()
        {
        }

        public void Initialize()
        {
            // Do nothing
        }

        public Point GetPosition()
        {
            return Simulator.Mouse.Position;
        }
    }
}
