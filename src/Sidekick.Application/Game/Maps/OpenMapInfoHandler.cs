using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Game.Maps.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Game.Maps
{
    public class OpenMapInfoHandler : ICommandHandler<OpenMapInfoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;

        public OpenMapInfoHandler(
            IMediator mediator,
            IViewLocator viewLocator,
            IClipboardProvider clipboardProvider)
        {
            this.mediator = mediator;
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
        }

        public async Task<bool> Handle(OpenMapInfoCommand request, CancellationToken cancellationToken)
        {
            // Parses the item by copying the item under the cursor
            var item = await mediator.Send(new ParseItemCommand(await clipboardProvider.Copy()));

            if (item == null)
            {
                // If the item can't be parsed, show an error
                await viewLocator.Open(View.ParserError);
            }
            else if (item.Properties.MapTier == 0)
            {
                // If the item is not a map
                await viewLocator.Open(View.InvalidItemError);
            }
            else
            {
                // If the item can be parsed, show the view
                await viewLocator.Open(View.Map, item);
            }

            return true;
        }
    }
}
