using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ElectronCgi.DotNet;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Application;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Trade;
using Sidekick.Domain.Game.Trade.Models;
using Sidekick.Domain.Initialization.Commands;
using Sidekick.Domain.Settings;
using Sidekick.Infrastructure;
using Sidekick.Logging;
using Sidekick.Mapper;
using Sidekick.Mediator;
using Sidekick.Persistence;
using Sidekick.Presentation;

namespace Sidekick.Console
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var serviceProvider = InitializeServices();

                var mediator = serviceProvider.GetRequiredService<IMediator>();

                await mediator.Send(new InitializeCommand(true));

                var connection = new ConnectionBuilder().UsingEncoding(Encoding.UTF8).Build();

                connection.OnAsync<string, Item>("parse-item", async itemText =>
                {
                    return await mediator.Send(new ParseItemCommand(itemText));
                });

                var tradeSearchService = serviceProvider.GetRequiredService<ITradeSearchService>();
                connection.OnAsync<Item, TradeSearchResult<string>>("fetch", async item =>
                {
                    return await tradeSearchService.Search(item);
                });

                var settings = serviceProvider.GetRequiredService<ISidekickSettings>();
                connection.On("get-settings", () => settings);

                connection.Listen();
            }).Wait();
        }

        public static ServiceProvider InitializeServices()
        {
            var services = new ServiceCollection()

                // Building blocks
                .AddSidekickLogging()
                .AddSidekickMapper(
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"))
                .AddSidekickMediator(
                    Assembly.Load("Sidekick.Application"),
                    Assembly.Load("Sidekick.Domain"),
                    Assembly.Load("Sidekick.Infrastructure"),
                    Assembly.Load("Sidekick.Persistence"),
                    Assembly.Load("Sidekick.Presentation"),
                    Assembly.Load("Sidekick.Presentation.Wpf"))

                // Layers
                .AddSidekickApplication()
                .AddSidekickInfrastructure()
                .AddSidekickPersistence()
                .AddSidekickPresentation();

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.UseSidekickMapper();

            return serviceProvider;
        }
    }
}
