using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sidekick.Domain.Views;
using Sidekick.Mediator;

namespace Sidekick.Presentation.Wpf.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMediatorTasks mediator;

        public ViewLocator(
            IServiceProvider serviceProvider,
            IMediatorTasks mediator)
        {
            this.serviceProvider = serviceProvider;
            this.mediator = mediator;
            Views = new List<ViewInstance>();
        }

        public List<ViewInstance> Views { get; set; }

        public void Open(View view, params object[] args)
        {
            Views.Add(new ViewInstance(this, serviceProvider, view, args));
        }

        public bool IsOpened(View view) => Views.Any(x => x.View.View == view && x.View.IsVisible);

        public bool IsAnyOpened() => Views.Any();

        public void CloseAll()
        {
            var views = Views.Where(x => x.View.IsVisible).ToList();
            foreach (var view in views)
            {
                view.View.Hide();
            }

            Task.Run(async () =>
            {
                await mediator.WhenAll;

                var views = Views.Where(x => !x.View.IsVisible).ToList();
                foreach (var view in views)
                {
                    view.View.Close();
                }
            });
        }

        public void Close(View view)
        {
            var views = Views.Where(x => x.View.View == view && x.View.IsVisible).ToList();
            foreach (var item in views)
            {
                item.View.Hide();
            }

            Task.Run(async () =>
            {
                await mediator.WhenAll;

                var views = Views.Where(x => x.View.View == view && !x.View.IsVisible).ToList();
                foreach (var item in views)
                {
                    item.View.Close();
                }
            });
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            CloseAll();
        }
    }
}
