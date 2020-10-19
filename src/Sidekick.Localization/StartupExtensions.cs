using System;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Settings;

namespace Sidekick.Localization
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickLocalization(this IServiceCollection services)
        {
            // Http Services
            services.AddHttpClient();

            services.AddSingleton<IUILanguageProvider, UILanguageProvider>();

            return services;
        }

        public static void UseSidekickLocalization(this IServiceProvider serviceProvider)
        {
            var settings = serviceProvider.GetRequiredService<SidekickSettings>();

            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo(settings.Language_UI);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(settings.Language_UI);
        }
    }
}
