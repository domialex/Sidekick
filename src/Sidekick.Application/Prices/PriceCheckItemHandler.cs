using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Prices.Commands;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Prices
{
    public class PriceCheckItemHandler : ICommandHandler<PriceCheckItemCommand, bool>
    {
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;
        private readonly IMediator mediator;

        public PriceCheckItemHandler(
            IViewLocator viewLocator,
            IClipboardProvider clipboardProvider,
            IMediator mediator)
        {
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
            this.mediator = mediator;
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
                item = await mediator.Send(new ParseItemCommand(itemText));
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
