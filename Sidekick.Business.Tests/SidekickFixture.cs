using AutoFixture;
using AutoFixture.AutoMoq;
using Serilog;
using Sidekick.Business.Languages;
using Sidekick.Business.Tokenizers;
using Sidekick.Business.Tokenizers.ItemName;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Tests
{
    public class SidekickFixture : Fixture
    {
        public SidekickFixture()
        {
            this.Customize(new AutoMoqCustomization());

            this.Register<ITokenizer>(this.Create<ItemNameTokenizer>);
            this.Register<ILanguageProvider>(this.Create<LanguageProvider>);
            this.Register(DefaultSettings.CreateDefault);
        }
    }
}
