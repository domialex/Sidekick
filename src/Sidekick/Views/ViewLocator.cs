using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace Sidekick.Views
{
    public class ViewLocator : IViewLocator, IDisposable
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Dispatcher dispatcher;

        public ViewLocator(
            IServiceProvider serviceProvider,
            Dispatcher dispatcher)
        {
            this.serviceProvider = serviceProvider;
            this.dispatcher = dispatcher;
            Views = new List<IViewInstance>();
        }

        public List<IViewInstance> Views { get; set; }

        public void Open<TView>()
            where TView : ISidekickView
        {
            dispatcher.Invoke(() =>
            {
                Views.Add(new ViewInstance<TView>(this, serviceProvider));
            });
        }

        public bool IsOpened<TView>()
            where TView : ISidekickView
        {
            return Views.Any(x => x.Type == typeof(TView));
        }

        public void CloseAll()
        {
            dispatcher.Invoke(() =>
            {
                while (Views.Count > 0)
                {
                    Remove(Views[0]);
                }
            });
        }

        public void Close<TView>()
            where TView : ISidekickView
        {
            dispatcher.Invoke(() =>
            {
                while (Views.Any(x => x.Type == typeof(TView)))
                {
                    Remove(Views.FirstOrDefault(x => x.Type == typeof(TView)));
                }
            });
        }

        public void Remove(IViewInstance viewInstance)
        {
            viewInstance?.Dispose();
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
