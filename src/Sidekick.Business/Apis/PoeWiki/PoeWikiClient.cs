using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;

namespace Sidekick.Business.Apis.PoeWiki
{
    public class PoeWikiClient : IPoeWikiClient
    {
        private const string WIKI_BASE_URI = "https://pathofexile.gamepedia.com/";
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IMediator mediator;

        public PoeWikiClient(
            ILogger<PoeWikiClient> logger,
            ILanguageProvider languageProvider,
            IMediator mediator)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.mediator = mediator;
        }

        /// <summary>
        /// Attempts to generate and open the wiki link for the given item
        /// </summary>
        public async Task Open(Item item)
        {
            // Only available for english portal
            if (item == null || !languageProvider.IsEnglish)
            {
                return;
            }

            // Most items will open the basetype wiki link.
            // Does not work for unique items that are not identified.
            if (string.IsNullOrEmpty(item.Name))
            {
                logger.LogWarning("Unable to open POE Wiki for specified item as it has no name! {@item}", item);
                return;
            }

            // determine search link, so wiki can be opened for any item
            var searchLink = item.Name ?? item.Type;
            // replace space encodes with '_' to match the link layout of the poe wiki and then url encode it
            var itemLink = System.Net.WebUtility.UrlEncode(searchLink.Replace(" ", "_"));

            await mediator.Send(new OpenBrowserCommand(new Uri(WIKI_BASE_URI + itemLink)));
        }
    }
}
