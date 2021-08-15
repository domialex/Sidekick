using Microsoft.Extensions.DependencyInjection;
using Sidekick.Apis.Poe.Clients;
using Sidekick.Apis.Poe.Metadatas;
using Sidekick.Apis.Poe.Modifiers;
using Sidekick.Apis.Poe.Parser;
using Sidekick.Apis.Poe.Parser.Patterns;
using Sidekick.Apis.Poe.Pseudo;
using Sidekick.Apis.Poe.Static;
using Sidekick.Apis.Poe.Translations.Stats;

namespace Sidekick.Apis.Poe
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPoeApi(this IServiceCollection services)
        {
            services.AddTransient<IPoeTradeClient, PoeTradeClient>();

            services.AddSingleton<IItemParser, ItemParser>();
            services.AddSingleton<IItemMetadataProvider, ItemMetadataProvider>();
            services.AddSingleton<IModifierProvider, ModifierProvider>();
            services.AddSingleton<IPseudoModifierProvider, PseudoModifierProvider>();
            services.AddSingleton<IItemStaticDataProvider, ItemStaticDataProvider>();
            services.AddSingleton<IStatTranslationProvider, StatTranslationProvider>();
            services.AddSingleton<IParserPatterns, ParserPatterns>();

            return services;
        }
    }
}
