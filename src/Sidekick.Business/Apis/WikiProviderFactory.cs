using System.Threading.Tasks;
using Sidekick.Business.Apis.PoeDb;
using Sidekick.Business.Apis.PoeWiki;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Settings;

namespace Sidekick.Business.Apis
{
    public class WikiProviderFactory : IWikiProvider
    {
        private readonly ISidekickSettings settings;
        private readonly IPoeWikiClient poeWikiClient;
        private readonly IPoeDbClient poeDbClient;

        public WikiProviderFactory(
            ISidekickSettings settings,
            IPoeWikiClient poeWikiClient,
            IPoeDbClient poeDbClient)
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
