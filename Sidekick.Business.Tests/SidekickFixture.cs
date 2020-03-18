using AutoFixture;
using AutoFixture.AutoMoq;
using Sidekick.Business.Languages;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Tests
{
    public class SidekickFixture : Fixture
    {
        public SidekickFixture()
        {
            this.Customize(new AutoMoqCustomization());

            this.Register<ILanguageProvider>(this.Create<LanguageProvider>);
            this.Register(DefaultSettings.CreateDefault);
        }
    }
}
