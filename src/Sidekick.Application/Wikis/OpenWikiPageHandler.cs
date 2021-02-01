using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Settings;
using Sidekick.Domain.Views;
using Sidekick.Domain.Wikis;
using Sidekick.Domain.Wikis.Commands;

namespace Sidekick.Application.Wikis
{
    public class OpenWikiPageHandler : ICommandHandler<OpenWikiPageCommand, bool>
    {
        private readonly IClipboardProvider clipboardProvider;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly IMediator mediator;
        private readonly IViewLocator viewLocator;
        private readonly ISidekickSettings settings;

        public OpenWikiPageHandler(
            IClipboardProvider clipboardProvider,
            IGameLanguageProvider gameLanguageProvider,
            IMediator mediator,
            IViewLocator viewLocator,
            ISidekickSettings settings)
        {
            this.clipboardProvider = clipboardProvider;
            this.gameLanguageProvider = gameLanguageProvider;
            this.mediator = mediator;
            this.viewLocator = viewLocator;
            this.settings = settings;
        }

        public async Task<bool> Handle(OpenWikiPageCommand request, CancellationToken cancellationToken)
        {
            var text = await clipboardProvider.Copy();
            var item = await mediator.Send(new ParseItemCommand(text));

            if (item == null)
            {
                // If the item can't be parsed, show an error
                await viewLocator.Open(View.ParserError);
                return false;
            }

            if (!gameLanguageProvider.IsEnglish)
            {
                // Only available for english language
                await viewLocator.Open(View.AvailableInEnglishError);
                return false;
            }

            if (string.IsNullOrEmpty(item.Name))
            {
                // Most items will open the basetype wiki link.
                // Does not work for unique items that are not identified.
                await viewLocator.Open(View.InvalidItemError);
                return false;
            }

            if (settings.Wiki_Preferred == WikiSetting.PoeDb)
            {
                await OpenPoeDb(item);
            }
            else
            {
                await OpenPoeWiki(item);
            }

            return true;
        }

        #region PoeDb
        private const string PoeDb_BaseUri = "https://poedb.tw/";
        private const string PoeDb_SubUrlUnique = "unique.php?n=";
        private const string PoeDb_SubUrlGem = "gem.php?n=";
        private const string PoeDb_SubUrlItem = "item.php?n=";
        private Task OpenPoeDb(Item item)
        {
            var subUrl = item.Rarity switch
            {
                Rarity.Unique => PoeDb_SubUrlUnique,
                Rarity.Gem => PoeDb_SubUrlGem,
                _ => PoeDb_SubUrlItem
            };

            var searchLink = item.Name ?? item.Type;
            var wikiLink = subUrl + searchLink.Replace(" ", "+");

            return mediator.Send(new OpenBrowserCommand(new Uri(PoeDb_BaseUri + wikiLink)));
        }
        #endregion

        #region PoeWiki
        private Task OpenPoeWiki(Item item)
        {
            // determine search link, so wiki can be opened for any item
            var searchLink = item.Name ?? item.Type;
            // replace space encodes with '_' to match the link layout of the poe wiki and then url encode it
            var itemLink = System.Net.WebUtility.UrlEncode(searchLink.Replace(" ", "_"));

            return mediator.Send(new OpenBrowserCommand(new Uri($"https://pathofexile.gamepedia.com/{itemLink}")));
        }
        #endregion
    }
}
