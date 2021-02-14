using System;
using Sidekick.Domain.Platforms;

namespace Sidekick.Presentation.Blazor.Mock
{
    public class KeybindProvider : IKeybindProvider
    {
        public event Func<string, bool> OnKeybind;

        public void Register()
        {
            // Do nothing in mock
        }

        public void Unregister()
        {
            // Do nothing in mock
        }
    }
}
