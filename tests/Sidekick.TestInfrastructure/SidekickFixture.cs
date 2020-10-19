using System.Net.Http;
using AutoFixture;
using AutoFixture.AutoMoq;
using Sidekick.Business.Apis.Poe.Parser.Patterns;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats;
using Sidekick.Business.Apis.Poe.Trade.Data.Stats.Pseudo;
using Sidekick.Business.Caches;
using Sidekick.Business.Languages;
using Sidekick.Core.Settings;
using Sidekick.TestInfrastructure.TestClients.Caches;
using Sidekick.TestInfrastructure.TestClients.PoeApi;

namespace Sidekick.TestInfrastructure
{
    public class SidekickFixture : Fixture
    {
        public SidekickFixture()
        {
            Customize(new AutoMoqCustomization());

            this.Register<ILanguageProvider>(this.Create<LanguageProvider>);
            this.Register<IPoeTradeClient>(this.Create<CachedPoeTradeClient>);
            this.Register<IHttpClientFactory>(this.Create<HttpClientFactory>);
            this.Register(DefaultSettings.CreateDefault);

            this.Register<ICacheService>(this.Create<TestCacheService>);
            this.Register<IItemDataService>(this.Create<ItemDataService>);
            this.Register<IParserPatterns>(this.Create<ParserPatterns>);
            this.Register<IPseudoStatDataService>(this.Create<PseudoStatDataService>);
            this.Register<IStatDataService>(this.Create<StatDataService>);


        }
    }
}
