using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Trade;
using Sidekick.Domain.Settings;

namespace Sidekick.Application.Game.Trade
{
    public class OpenTradePageHandler : ICommandHandler<OpenTradePageCommand, bool>
    {
        private readonly IClipboardProvider clipboardProvider;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IMediator mediator;
        private readonly ISidekickSettings settings;

        public OpenTradePageHandler(
            IClipboardProvider clipboardProvider,
            IGameLanguageProvider gameLanguageProvider,
            ITradeSearchService tradeSearchService,
            IMediator mediator,
            ISidekickSettings settings)
        {
            this.clipboardProvider = clipboardProvider;
            this.gameLanguageProvider = gameLanguageProvider;
            this.tradeSearchService = tradeSearchService;
            this.mediator = mediator;
            this.settings = settings;
        }

        public async Task<bool> Handle(OpenTradePageCommand request, CancellationToken cancellationToken)
        {
            var text = await clipboardProvider.Copy();
            var item = await mediator.Send(new ParseItemCommand(text));

            if (item != null)
            {
                string id;

                if (item.Category == Category.Currency)
                {
                    var result = await tradeSearchService.SearchBulk(item);
                    id = result.Id;
                }
                else
                {
                    var result = await tradeSearchService.Search(item);
                    id = result.Id;
                }

                await mediator.Send(new OpenBrowserCommand(new Uri($"{gameLanguageProvider.Language.PoeTradeSearchBaseUrl}{settings.LeagueId}/{id}")));
                return true;
            }

            return false;
        }
    }
}
