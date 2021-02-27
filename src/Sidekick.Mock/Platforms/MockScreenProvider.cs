using System.Drawing;
using Sidekick.Domain.Platforms;

namespace Sidekick.Mock.Platforms
{
    public class MockScreenProvider : IScreenProvider
    {
        public Rectangle GetBounds()
        {
            return new Rectangle(0, 0, 1920, 1080);
        }

        public void Initialize()
        {
            // Do nothing in mock
        }
    }
}
