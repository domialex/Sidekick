using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.UI.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IServiceProvider serviceProvider;

        public ViewLocator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;

            Views = new List<ViewInstance>();
        }

        public List<ViewInstance> Views { get; set; }

        public void Open<TView>()
            where TView : ISidekickView
        {
            var view = new ViewInstance(
                serviceProvider.CreateScope(),
                typeof(TView)
            );

            view.Disposed += () =>
            {
                Views.Remove(view);
            };

            Views.Add(view);
        }

        public void Dispose()
        {
            if (Views != null)
            {
                foreach (var view in Views)
                {
                    view.Dispose();
                }
            }
        }
    }
}
