using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Business.Apis.Poe.Trade.Search;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Prices.Commands;

namespace Sidekick.Application.Prices
{
    public class OpenTradePageHandler : ICommandHandler<OpenTradePageCommand, bool>
    {
        private readonly IClipboardProvider clipboardProvider;
        private readonly IParserService parserService;
        private readonly ITradeSearchService tradeSearchService;

        public OpenTradePageHandler(
            IClipboardProvider clipboardProvider,
            IParserService parserService,
            ITradeSearchService tradeSearchService)
        {
            this.clipboardProvider = clipboardProvider;
            this.parserService = parserService;
            this.tradeSearchService = tradeSearchService;
        }

        public async Task<bool> Handle(OpenTradePageCommand request, CancellationToken cancellationToken)
        {
            var text = await clipboardProvider.Copy();
            var item = parserService.ParseItem(text);

            if (item != null)
            {
                await tradeSearchService.OpenWebpage(item);
                return true;
            }

            return false;
        }
    }
}
