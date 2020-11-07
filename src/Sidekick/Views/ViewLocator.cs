using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using Sidekick.Domain.Views;
using Sidekick.Mediator;

namespace Sidekick.Views
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

        public void Open(View view)
        {
            dispatcher.Invoke(() =>
            {
                Views.Add(new ViewInstance(this, view, serviceProvider));
            });
        }

        public bool IsOpened(View view) => Views.Any(x => x.View == view);

        public bool IsAnyOpened() => Views.Any();

        public void CloseAll()
        {
            dispatcher.Invoke(async () =>
            {
                foreach (var view in Views)
                {
                    view.WpfView.Hide();
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
            dispatcher.Invoke(async () =>
            {
                foreach (var view in Views.Where(x => x.View == view))
                {
                    view.WpfView.Hide();
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
