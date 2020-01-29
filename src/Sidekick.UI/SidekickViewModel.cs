using System;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Languages.UI;

namespace Sidekick.UI
{
    public abstract class SidekickViewModel
    {
        public SidekickViewModel(IServiceProvider serviceProvider)
        {
            var uiLanguageProvider = serviceProvider.GetService<IUILanguageProvider>();

            var cultureInfo = new CultureInfo(uiLanguageProvider.Current.Name);
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
        }
    }
}
