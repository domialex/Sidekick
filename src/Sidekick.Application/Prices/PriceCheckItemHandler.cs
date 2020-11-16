using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Prices.Commands;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Prices
{
    public class PriceCheckItemHandler : ICommandHandler<PriceCheckItemCommand, bool>
    {
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;
        private readonly IParserService parserService;

        public PriceCheckItemHandler(
            IViewLocator viewLocator,
            IClipboardProvider clipboardProvider,
            IParserService parserService)
        {
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
            this.parserService = parserService;
        }

        public async Task<bool> Handle(PriceCheckItemCommand request, CancellationToken cancellationToken)
        {
            // Close previously opened price views
            viewLocator.Close(View.ParserError);
            viewLocator.Close(View.Price);

            // Parses the item by copying the item under the cursor
            var item = request.Item;
            if (item == null)
            {
                var itemText = await clipboardProvider.Copy();
                item = parserService.ParseItem(itemText);
            }

            if (item == null)
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
