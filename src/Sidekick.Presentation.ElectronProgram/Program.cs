using System;
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

namespace Sidekick.Presentation.ElectronProgram
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

                var item = await mediator.Send(new ParseItemCommand(TestItem));
                Console.WriteLine(item.Name);

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
                    Assembly.Load("Sidekick.Persistence"))

                // Layers
                .AddSidekickApplication()
                .AddSidekickInfrastructure()
                .AddSidekickPersistence();

            var serviceProvider = services.BuildServiceProvider();

            serviceProvider.UseSidekickMapper();

            return serviceProvider;
        }

        private static string TestItem = @"Rarity: Unique
Starkonja's Head
Silken Hood
--------
Quality: +20% (augmented)
Evasion Rating: 810 (augmented)
--------
Requirements:
Level: 70
Dex: 138
Int: 140
--------
Sockets: G-G-B-B 
--------
Item Level: 61
--------
Elemental Hit deals 40% increased Damage
--------
+67 to Dexterity
50% reduced Damage when on Low Life
10% increased Attack Speed
25% increased Global Critical Strike Chance
114% increased Evasion Rating
+99 to maximum Life
150% increased Global Evasion Rating when on Low Life
--------
There was no hero made out of Starkonja's death,
but merely a long sleep made eternal.";
    }
}
