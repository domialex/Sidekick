using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;

namespace Sidekick.Business.Apis.PoeDb
{
    /// <summary>
    /// https://poedb.tw/us/
    /// Only in English.
    /// </summary>
    public class PoeDbClient : IPoeDbClient
    {
        private const string PoeDbBaseUri = "https://poedb.tw/";
        private const string SubUrlUnique = "unique.php?n=";
        private const string SubUrlGem = "gem.php?n=";
        private const string SubUrlItem = "item.php?n=";
        private readonly ILogger logger;
        private readonly ILanguageProvider languageProvider;
        private readonly IMediator mediator;

        public PoeDbClient(ILogger<PoeDbClient> logger,
            ILanguageProvider languageProvider,
            IMediator mediator)
        {
            this.logger = logger;
            this.languageProvider = languageProvider;
            this.mediator = mediator;
        }

        public async Task Open(Item item)
        {
            if (item == null || !languageProvider.IsEnglish)
            {
                return;
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                logger.LogWarning("Unable to open PoeDB for specified item as it has no name! {@item}", item);
                return;
            }

            var subUrl = item.Rarity switch
            {
                Rarity.Unique => SubUrlUnique,
                Rarity.Gem => SubUrlGem,
                _ => SubUrlItem
            };

            var searchLink = item.Name ?? item.Type;
            var wikiLink = subUrl + searchLink.Replace(" ", "+");

            await mediator.Send(new OpenBrowserCommand(new Uri(PoeDbBaseUri + wikiLink)));
        }
    }
}
