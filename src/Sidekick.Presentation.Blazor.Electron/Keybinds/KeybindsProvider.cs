using System;
using Sidekick.Domain.Keybinds;

namespace Sidekick.Presentation.Blazor.Electron.Keybinds
{
    public class KeybindsProvider : IKeybindsProvider, IDisposable
    {
        public KeybindsProvider()
        {
        }

        public event Func<string, bool> OnKeyDown;
        public event Func<bool> OnScrollDown;
        public event Func<bool> OnScrollUp;

        public void Initialize()
        {
        }

        public void PressKey(string keys)
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
