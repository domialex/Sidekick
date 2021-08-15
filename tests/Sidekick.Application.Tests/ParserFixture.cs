using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Apis.Poe;
using Sidekick.Application.Initialization;
using Sidekick.Common;
using Sidekick.Common.Extensions;
using Sidekick.Common.Platform;
using Sidekick.Common.Settings;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Views;
using Sidekick.Infrastructure;
using Sidekick.Localization;
using Sidekick.Mapper;
using Sidekick.Mediator;
using Sidekick.Mock.Platforms;
using Sidekick.Mock.Views;
using Sidekick.Modules.Settings;
using Sidekick.Persistence;
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
                .AddSidekickMapper()
                .AddSidekickMediator(
                    typeof(InitializeCommand).Assembly,
                    typeof(InitializeHandler).Assembly
                )

                // Layers
                .AddSidekickApplication()
                .AddSidekickInfrastructure()
                .AddSidekickLocalization()
                .AddSidekickPersistence()

                // Apis
                .AddSidekickPoeApi()

                // Modules
                .AddSidekickSettings(configuration);

            services.AddSingleton<IViewLocator, MockViewLocator>();
            services.AddSingleton<IKeybindProvider, MockKeybindProvider>();
            services.AddSingleton<IKeyboardProvider, MockKeyboardProvider>();
            services.AddSingleton<IProcessProvider, MockProcessProvider>();

            var serviceProvider = services.BuildServiceProvider();

            var mediator = serviceProvider.GetRequiredService<IMediator>();
            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            await settingsService.Save(nameof(ISettings.Language_Parser), "en");
            await settingsService.Save(nameof(ISettings.Language_UI), "en");
            await settingsService.Save(nameof(ISettings.LeagueId), "Standard");
            await mediator.Send(new InitializeCommand(true, false));

            Parser = serviceProvider.GetRequiredService<IItemParser>();
        }
    }
}
