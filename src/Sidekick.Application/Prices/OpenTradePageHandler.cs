using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Prices.Commands;

namespace Sidekick.Application.Prices
{
    public class OpenTradePageHandler : ICommandHandler<OpenTradePageCommand, bool>
    {
        private readonly IClipboardProvider clipboardProvider;
        private readonly ITradeSearchService tradeSearchService;
        private readonly IMediator mediator;

        public OpenTradePageHandler(
            IClipboardProvider clipboardProvider,
            ITradeSearchService tradeSearchService,
            IMediator mediator)
        {
            this.clipboardProvider = clipboardProvider;
            this.tradeSearchService = tradeSearchService;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(OpenTradePageCommand request, CancellationToken cancellationToken)
        {
            var text = await clipboardProvider.Copy();
            var item = await mediator.Send(new ParseItemCommand(text));

            if (item != null)
            {
                await tradeSearchService.OpenWebpage(item);
                return true;
            }

            return false;
        }
    }
}
