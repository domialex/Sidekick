using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Localization;

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

        private List<ViewInstance> Views { get; set; }

        public void Open<TView>()
            where TView : ISidekickView
        {
            var uiLanguageProvider = serviceProvider.GetService<IUILanguageProvider>();

            var cultureInfo = new CultureInfo(uiLanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;

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

        public bool IsOpened<TView>()
        {
            return Views.Any(x => x.ViewType == typeof(TView));
        }

        public void CloseAll()
        {
            for (var i = Views.Count; i > 0; i--)
            {
                Views[i - 1].View.Close();
            }
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
