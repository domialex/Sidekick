using System;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Common.Settings;
using Sidekick.Domain.App.Commands;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Trade;
using Sidekick.Domain.Keybinds;
using Sidekick.Domain.Platforms;

namespace Sidekick.Application.Keybinds
{
    public class OpenTradePageKeybindHandler : IKeybindHandler
    {
        private readonly IClipboardProvider clipboardProvider;
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IMediator mediator;
        private readonly ISettings settings;
        private readonly IProcessProvider processProvider;

        public OpenTradePageKeybindHandler(
            IClipboardProvider clipboardProvider,
            IGameLanguageProvider gameLanguageProvider,
            ITradeSearchService tradeSearchService,
            IMediator mediator,
            ISettings settings,
            IProcessProvider processProvider)
        {
            this.clipboardProvider = clipboardProvider;
            this.gameLanguageProvider = gameLanguageProvider;
            this.tradeSearchService = tradeSearchService;
            this.mediator = mediator;
            this.settings = settings;
            this.processProvider = processProvider;
        }

        public bool IsValid() => processProvider.IsPathOfExileInFocus;

        public async Task Execute()
        {
            var text = await clipboardProvider.Copy();
            var item = await mediator.Send(new ParseItemCommand(text));

            if (item != null)
            {
                string id;

                if (item.Metadata.Category == Category.Currency)
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
            }
        }
    }
}
