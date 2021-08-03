using System.Reflection;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Sidekick.Common.Platform;
using Sidekick.Common.Settings;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Views;
using Sidekick.Extensions;
using Sidekick.Infrastructure;
using Sidekick.Localization;
using Sidekick.Logging;
using Sidekick.Mapper;
using Sidekick.Mediator;
using Sidekick.Mock.Platforms;
using Sidekick.Mock.Views;
using Sidekick.Modules.Settings;
using Sidekick.Persistence;
using Xunit;

namespace Sidekick.Application.Tests
{
    public class MediatorFixture : IAsyncLifetime
    {
        public IMediator Mediator { get; private set; }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        public async Task InitializeAsync()
        {
            var mockEnvironment = new Mock<IHostEnvironment>();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(SidekickPaths.GetDataFilePath(SettingsService.FileName), true, true)
                .Build();

            var services = new ServiceCollection()

                // Building blocks
                .AddSidekickLogging(configuration, mockEnvironment.Object)
                .AddSidekickMapper(
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"))
                .AddSidekickMediator(
                    Assembly.Load("Sidekick.Application"),
                    Assembly.Load("Sidekick.Domain"),
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Mock"),
                    Assembly.Load("Sidekick.Persistence"))

                // Layers
                .AddSidekickApplication(configuration)
                .AddSidekickInfrastructure()
                .AddSidekickLocalization()
                .AddSidekickPersistence()

                // Modules
                .AddSidekickSettings(configuration);

            services.AddSingleton<IViewLocator, MockViewLocator>();
            services.AddSingleton<IKeybindProvider, MockKeybindProvider>();
            services.AddSingleton<IKeyboardProvider, MockKeyboardProvider>();
            services.AddSingleton<IProcessProvider, MockProcessProvider>();

            var serviceProvider = services.BuildServiceProvider();

            Mediator = serviceProvider.GetRequiredService<IMediator>();
            var settingsService = serviceProvider.GetRequiredService<ISettingsService>();
            await settingsService.Save(nameof(ISettings.Language_Parser), "en");
            await settingsService.Save(nameof(ISettings.Language_UI), "en");
            await settingsService.Save(nameof(ISettings.LeagueId), "Standard");
            await Mediator.Send(new InitializeCommand(true, false));
        }
    }
}
