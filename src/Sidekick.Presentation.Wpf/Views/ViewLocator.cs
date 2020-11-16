using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Sidekick.Domain.Views;
using Sidekick.Mediator;

namespace Sidekick.Presentation.Wpf.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IMediatorTasks mediator;
        private readonly Dispatcher dispatcher;

        public ViewLocator(
            IServiceProvider serviceProvider,
            IMediatorTasks mediator,
            Dispatcher dispatcher)
        {
            this.serviceProvider = serviceProvider;
            this.mediator = mediator;
            this.dispatcher = dispatcher;
            Views = new List<ViewInstance>();
        }

        public List<ViewInstance> Views { get; set; }

        public void Open(View view, params object[] args)
        {
            dispatcher.Invoke(() =>
            {
                Views.Add(new ViewInstance(this, serviceProvider, view, args));
            });
        }

        public bool IsOpened(View view) => Views.Any(x => x.View == view && x.WpfView.IsVisible);

        public bool IsAnyOpened() => Views.Any();

        public void CloseAll()
        {
            Task.Run(async () =>
            {
                for (var i = 0; i < Views.Count; i++)
                {
                    Views[i].WpfView.Hide();
                }

                await mediator.WhenAll;

                while (Views.Count > 0)
                {
                    Views[0].WpfView.Close();
                }
            });
        }

        public void Close(View view)
        {
            Task.Run(async () =>
            {
                var views = Views.Where(x => x.View == view);
                var count = views.Count();
                for (var i = 0; i < count; i++)
                {
                    views.ElementAt(i).WpfView.Hide();
                }
                await mediator.WhenAll;
                while (Views.Any(x => x.View == view))
                {
                    Views.FirstOrDefault(x => x.View == view)?.WpfView.Close();
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
