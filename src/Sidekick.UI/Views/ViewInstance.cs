using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.UI.Views
{
    public class ViewInstance : IDisposable
    {
        public ViewInstance(IServiceScope scope, Type viewType)
        {
            Scope = scope;
            View = (ISidekickView)scope.ServiceProvider.GetService(viewType);
            View.Closed += View_Closed;
        }

        public IServiceScope Scope { get; private set; }

        public ISidekickView View { get; private set; }

        public event Action Disposed;

        private void View_Closed(object sender, EventArgs e)
        {
            Dispose();
        }

        public void Dispose()
        {
            View.Closed -= View_Closed;
            Scope?.Dispose();
            Disposed?.Invoke();
        }
    }
}
