using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Apis.Poe;
using Sidekick.Common;
using Sidekick.Common.Blazor.Views;
using Sidekick.Common.Game;
using Sidekick.Common.Platform;
using Sidekick.Common.Settings;
using Sidekick.Mock;
using Sidekick.Modules.Initialization;
using Sidekick.Modules.Settings;
using Xunit;

namespace Sidekick.Application.Tests
{
    public class ParserFixture : IAsyncLifetime
    {
        public IItemParser Parser { get; private set; }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(SidekickPaths.GetDataFilePath(SettingsService.FileName), true, true)
                .Build();

            var services = new ServiceCollection()

                // Building blocks
                .AddSidekickCommon()
                .AddSidekickCommonGame()

                // Apis
                .AddSidekickPoeApi()

                // Modules
                .AddSidekickInitialization()
                .AddSidekickSettings(configuration);

            services.AddSingleton<IViewLocator, MockViewLocator>();
            services.AddSingleton<IKeybindProvider, MockKeybindProvider>();
            services.AddSingleton<IKeyboardProvider, MockKeyboardProvider>();
            services.AddSingleton<IProcessProvider, MockProcessProvider>();

            var serviceProvider = services.BuildServiceProvider();

            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            await settingsService.Save(nameof(ISettings.Language_Parser), "en");
            await settingsService.Save(nameof(ISettings.Language_UI), "en");
            await settingsService.Save(nameof(ISettings.LeagueId), "Standard");

            var component = new Sidekick.Modules.Initialization.Pages.Initialization();
            await component.Handle();

            Parser = serviceProvider.GetRequiredService<IItemParser>();
        }
    }
}
