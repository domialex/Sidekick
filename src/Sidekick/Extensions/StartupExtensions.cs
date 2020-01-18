using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Business.Http;
using Sidekick.Business.Languages;
using Sidekick.Business.Loggers;
using Sidekick.Business.Notifications;
using Sidekick.Business.Parsers;
using Sidekick.Business.Tokenizers;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Business.Trades;

namespace Sidekick.Core.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickServices(this IServiceCollection services)
        {
            services.AddSingleton<IItemParser, ItemParser>();
            services.AddSingleton<ITradeClient, TradeClient>();
            services.AddSingleton<ILanguageProvider, LanguageProvider>();

            services.AddSingleton<IUILanguageProvider, UILanguageProvider>();
            services.AddSingleton<INotificationService, NotificationService>();

            services.AddSingleton<ILogger, Logger>();
            services.AddSingleton<IHttpClientProvider, HttpClientProvider>();

            services.AddSingleton<ITokenizer, ItemNameTokenizer>();

            return services;
        }

        public static IServiceCollection AddSidekickConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            // Add configuration files here

            return services;
        }
    }
}
