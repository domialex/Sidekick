using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using ElectronCgi.DotNet;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Sidekick.Business;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Core;
using Sidekick.Core.Initialization;
using Sidekick.Core.Natives;
using Sidekick.Core.Settings;
using Sidekick.Database;
using Sidekick.Localization;

namespace Sidekick.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var serviceProvider = InitializeServices();

                var parserService = serviceProvider.GetRequiredService<IParserService>();
                var initializer = serviceProvider.GetRequiredService<IInitializer>();

                await initializer.Initialize();

                var connection = new ConnectionBuilder().UsingEncoding(Encoding.UTF8).Build();

                connection.OnAsync<string, Item>("parse-item", async itemText =>
                {
                    return await parserService.ParseItem(itemText);
                });

                var tradeSearchService = serviceProvider.GetRequiredService<ITradeSearchService>();
                connection.OnAsync<Item, FetchResult<string>>("fetch", async item =>
                {
                    return await tradeSearchService.Search(item);
                });

                var settings = serviceProvider.GetRequiredService<SidekickSettings>();
                connection.On("get-settings", () => settings);

                connection.Listen();
            }).GetAwaiter().GetResult();
        }

        public static ServiceProvider InitializeServices()
        {
            var services = new ServiceCollection()
               .AddSingleton<INativeBrowser, NativeBrowser>()
               .AddSidekickConfiguration()
               .AddSidekickCoreServices()
               .AddSidekickBusinessServices()
               .AddSidekickLocalization()
               .AddSidekickDatabase();

            return services.BuildServiceProvider();
        }
    }

    public class NativeBrowser : INativeBrowser
    {
        private readonly ILogger logger;

        public NativeBrowser(ILogger logger)
        {
            this.logger = logger.ForContext(GetType());
        }

        public void Open(Uri uri)
        {
            logger.Information("Opening in browser: {uri}", uri.AbsoluteUri);
            var psi = new ProcessStartInfo
            {
                FileName = uri.AbsoluteUri,
                UseShellExecute = true
            };
            Process.Start(psi);
        }
    }
}
