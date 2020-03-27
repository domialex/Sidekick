using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Views
{
    public class ViewInstance : IDisposable
    {
        private bool isDisposed;

        public ViewInstance(IServiceScope scope, Type viewType)
        {
            Scope = scope;
            ViewType = viewType;
            View = (ISidekickView)scope.ServiceProvider.GetService(viewType);
            View.Closed += View_Closed;
        }

        public IServiceScope Scope { get; private set; }

        public ISidekickView View { get; private set; }

        public Type ViewType { get; private set; }

        public event Action Disposed;

        private void View_Closed(object sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                View.Closed -= View_Closed;
                Scope?.Dispose();
                Disposed?.Invoke();
            }

            isDisposed = true;
        }
    }
}
