using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application.Game.Items.Parser.Patterns;
using Sidekick.Application.Game.Languages;
using Sidekick.Application.Keybinds;
using Sidekick.Application.Settings;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Settings;

namespace Sidekick.Application
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var sidekickConfiguration = new SidekickSettings();
            configuration.Bind(sidekickConfiguration);
            configuration.BindList(nameof(SidekickSettings.Chat_Commands), sidekickConfiguration.Chat_Commands);
            services.AddSingleton(sidekickConfiguration);

            services.AddSingleton<IGameLanguageProvider, GameLanguageProvider>();
            services.AddSingleton<ISidekickSettings>(sp => sp.GetRequiredService<SidekickSettings>());
            services.AddSingleton<IParserPatterns, ParserPatterns>();

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

        public static void BindList<TModel>(this IConfiguration configuration, string key, List<TModel> list)
            where TModel : new()
        {
            var items = configuration.GetSection(key).GetChildren().ToList();
            if (items.Count > 0)
            {
                list.Clear();
            }
            foreach (var item in items)
            {
                var model = new TModel();
                item.Bind(model);
                list.Add(model);
            }
        }
    }
}
