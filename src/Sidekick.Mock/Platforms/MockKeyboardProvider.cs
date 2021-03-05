#pragma warning disable CS0067

using System;
using Sidekick.Domain.Platforms;

namespace Sidekick.Mock.Platforms
{
    public class MockKeyboardProvider : IKeyboardProvider
    {
        public event Action<string> OnKeyDown;

        public void Initialize()
        {
            // Do nothing in mock
        }

        public bool IsCtrlPressed()
        {
            return true;
        }

        public void PressKey(params string[] keys)
        {
            // Do nothing in mock
        }

        public string ToElectronAccelerator(string key)
        {
            return key;
        }
    }
}
