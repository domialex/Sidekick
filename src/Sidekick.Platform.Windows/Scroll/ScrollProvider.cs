using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using NeatInput.Windows;
using NeatInput.Windows.Events;
using Sidekick.Domain.Platforms;

namespace Sidekick.Platform.Windows.Scroll
{
    public class ScrollProvider : IScrollProvider, IDisposable, IMouseEventReceiver
    {
        private readonly ILogger<ScrollProvider> logger;

        private InputSource InputSource { get; set; }

        public ScrollProvider(ILogger<ScrollProvider> logger)
        {
            this.logger = logger;
        }

        public event Func<bool> OnScrollDown;
        public event Func<bool> OnScrollUp;

        public void Initialize()
        {
            if (!Debugger.IsAttached)
            {
                InputSource = new InputSource(this);
                InputSource.Listen();
            }

#if DEBUG
            Dispose(true);
#endif
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (InputSource != null)
            {
                InputSource.Dispose();
            }
        }

        public void Receive(MouseEvent @event)
        {
            // TODO
        }
    }
}
