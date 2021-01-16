using System;
using Sidekick.Domain.Platforms;
using WindowsHook;

namespace Sidekick.Platform.Windows.Scroll
{
    public class ScrollProvider : IScrollProvider, IDisposable
    {
        public ScrollProvider()
        {
        }

        public IKeyboardMouseEvents Hook { get; private set; }

        public event Func<bool> OnScrollDown;
        public event Func<bool> OnScrollUp;

        public void Initialize()
        {
            Hook = WindowsHook.Hook.GlobalEvents();
            Hook.MouseWheelExt += Hook_MouseWheelExt;
        }

        private void Hook_MouseWheelExt(object sender, MouseEventExtArgs e)
        {
            if (e.Delta > 0)
            {
                e.Handled = OnScrollUp?.Invoke() ?? false;
            }
            else
            {
                e.Handled = OnScrollDown?.Invoke() ?? false;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Hook != null) // Hook will be null if auto update was successful
            {
                Hook.MouseWheelExt -= Hook_MouseWheelExt;
                Hook.Dispose();
            }
        }
    }
}
