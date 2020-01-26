using System;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.UI
{
    public abstract class SidekickViewModel<TView>
        where TView : ISidekickView
    {
        private readonly IServiceProvider serviceProvider;

        public SidekickViewModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        private IServiceScope Scope { get; set; }

        protected ISidekickView View { get; set; }

        public void Open()
        {
            Scope = serviceProvider.CreateScope();
            View = Scope.ServiceProvider.GetService<TView>();
            View.Open();
        }

        public void Close()
        {
            if (View != null)
            {
                View.Close();
            }

            if (Scope != null)
            {
                Scope.Dispose();
                Scope = null;
            }
        }
    }
}
