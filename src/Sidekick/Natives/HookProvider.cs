using System;
using WindowsHook;

namespace Sidekick.Natives
{
    public class HookProvider : IDisposable
    {
        private bool isDisposed;

        public HookProvider()
        {
            Hook = WindowsHook.Hook.GlobalEvents();
        }

        public IKeyboardMouseEvents Hook { get; private set; }

        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            if (Hook != null) // Hook will be null if auto update was successful
            {
                Hook.Dispose();
            }
            isDisposed = true;
            GC.SuppressFinalize(this);
        }
    }
}
