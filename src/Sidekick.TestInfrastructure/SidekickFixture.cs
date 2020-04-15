using AutoFixture;
using AutoFixture.AutoMoq;
using Sidekick.Business.Apis.Poe.Trade;
using Sidekick.Business.Apis.Poe.Trade.Data.Items;
using Sidekick.Business.Languages;
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
            this.Register<IItemDataService>(this.Create<ItemDataService>);
            this.Register<IPoeTradeClient>(this.Create<CachedPoeTradeClient>);
            this.Register(DefaultSettings.CreateDefault);
        }
    }
}
