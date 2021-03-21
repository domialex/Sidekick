using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Trade.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Game.Trade
{
    public class PriceCheckItemHandler : ICommandHandler<PriceCheckItemCommand, bool>
    {
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;

        public PriceCheckItemHandler(
            IViewLocator viewLocator,
            IClipboardProvider clipboardProvider)
        {
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
        }

        public async Task<bool> Handle(PriceCheckItemCommand request, CancellationToken cancellationToken)
        {
            var itemText = await clipboardProvider.Copy();

            await viewLocator.Open(View.Map, itemText);

            return true;
        }
    }
}
