using System.Drawing;
using Sidekick.Domain.Platforms;

namespace Sidekick.Mock.Platforms
{
    public class MockMouseProvider : IMouseProvider
    {
        public Point GetPosition()
        {
            return new Point(0, 0);
        }

        public void Initialize()
        {
            // Do nothing in mock
        }
    }
}
