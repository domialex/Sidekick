using System;
using Sidekick.Domain.Platforms;

namespace Sidekick.Mock.Platforms
{
    public class MockScrollProvider : IScrollProvider
    {
        public event Func<bool> OnScrollDown;
        public event Func<bool> OnScrollUp;

        public void Initialize()
        {
            // Do nothing in mock
        }
    }
}
