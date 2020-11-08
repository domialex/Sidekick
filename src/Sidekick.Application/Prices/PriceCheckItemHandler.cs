using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Prices.Commands;
using Sidekick.Domain.Views;
using Sidekick.Domain.Views.Commands;

namespace Sidekick.Application.Prices
{
    public class PriceCheckItemHandler : ICommandHandler<PriceCheckItemCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;
        private readonly IParserService parserService;

        public PriceCheckItemHandler(
            IMediator mediator,
            IViewLocator viewLocator,
            IClipboardProvider clipboardProvider,
            IParserService parserService)
        {
            this.mediator = mediator;
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
            this.parserService = parserService;
        }

        public async Task<bool> Handle(PriceCheckItemCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new CloseViewCommand());

            // Close previously opened price views
            viewLocator.Close(View.ParserError);
            viewLocator.Close(View.Price);

            // Parses the item by copying the item under the cursor
            var item = parserService.ParseItem(await clipboardProvider.Copy());

            if (item == null || item.Properties.MapTier == 0)
            {
                // If the item can't be parsed, show an error
                viewLocator.Open(View.ParserError);
            }
            else
            {
                // If the item can be parsed, show the view
                viewLocator.Open(View.Price, item);
            }

            return true;
        }
    }
}
