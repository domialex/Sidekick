using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application.Keybinds;

namespace Sidekick.Application
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickApplication(this IServiceCollection services)
        {
            // Keybind handlers
            services.AddSingleton<ChatKeybindHandler>();
            services.AddSingleton<CloseOverlayKeybindHandler>();
            services.AddSingleton<FindItemKeybindHandler>();
            services.AddSingleton<OpenCheatsheetsKeybindHandler>();
            services.AddSingleton<OpenMapInfoKeybindHandler>();
            services.AddSingleton<OpenSettingsKeybindHandler>();
            services.AddSingleton<OpenTradePageKeybindHandler>();
            services.AddSingleton<OpenWikiPageKeybindHandler>();
            services.AddSingleton<PriceCheckItemKeybindHandler>();
            services.AddSingleton<ScrollStashDownKeybindHandler>();
            services.AddSingleton<ScrollStashUpKeybindHandler>();

            return services;
        }
    }
}
