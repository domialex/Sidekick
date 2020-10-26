using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;
using Sidekick.Business.Apis.PoeDb;
using Sidekick.Business.Apis.PoeWiki;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Apis
{
    public class WikiProviderFactory : IWikiProvider
    {
        private readonly SidekickSettings settings;
        private readonly IPoeWikiClient poeWikiClient;
        private readonly IPoeDbClient poeDbClient;

        public WikiProviderFactory(SidekickSettings settings, IPoeWikiClient poeWikiClient, IPoeDbClient poeDbClient)
        {
            this.settings = settings;
            this.poeWikiClient = poeWikiClient;
            this.poeDbClient = poeDbClient;
        }

        public Task Open(Item item)
        {
            return GetCurrentProvider().Open(item);
        }

        private IWikiProvider GetCurrentProvider()
        {
            return settings.Wiki_Preferred == WikiSetting.PoeDb
                ? (IWikiProvider)poeDbClient
                : poeWikiClient;
        }
    }
}
