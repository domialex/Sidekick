using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application.Game.Items.Parser.Patterns;
using Sidekick.Application.Game.Languages;
using Sidekick.Application.Keybinds;
using Sidekick.Application.Settings;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Settings;

namespace Sidekick.Application
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var sidekickConfiguration = new SidekickSettings();
            configuration.Bind(sidekickConfiguration);
            services.AddSingleton(sidekickConfiguration);

            services.AddSingleton<IGameLanguageProvider, GameLanguageProvider>();
            services.AddSingleton<ISidekickSettings>(sp => sp.GetRequiredService<SidekickSettings>());
            services.AddSingleton<IKeybindsExecutor, KeybindsExecutor>();
            services.AddSingleton<IParserPatterns, ParserPatterns>();

            return services;
        }
    }
}
