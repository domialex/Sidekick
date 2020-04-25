using System.Net.Http;
using AutoFixture;
using AutoFixture.AutoMoq;
using Sidekick.Business.Apis.Poe.Parser.Patterns;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Languages;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;
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

            this.Register(GetInitializable<IItemDataService, ItemDataService>);
            this.Register(GetInitializable<IParserPatterns, ParserPatterns>);
        }

        // Simplified replacement for the initializer.
        // Works for now but may need to be reworked when new types of tests are added
        private TInterface GetInitializable<TInterface, TImplementation>() where TImplementation : TInterface
        {
            var instance = this.Create<TImplementation>();

            if (instance is IOnBeforeInit onBeforeInit)
            {
                onBeforeInit.OnBeforeInit().GetAwaiter().GetResult();
            }

            if(instance is IOnInit onInit)
            {
                onInit.OnInit().GetAwaiter().GetResult();
            }

            if (instance is IOnAfterInit onAfterInit)
            {
                onAfterInit.OnAfterInit().GetAwaiter().GetResult();
            }

            return instance;
        }
    }
}
