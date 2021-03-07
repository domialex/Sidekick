using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Maps.Commands;
using Sidekick.Domain.Platforms;
using Sidekick.Domain.Views;

namespace Sidekick.Application.Game.Maps
{
    public class OpenMapInfoHandler : ICommandHandler<OpenMapInfoCommand, bool>
    {
        private readonly IViewLocator viewLocator;
        private readonly IClipboardProvider clipboardProvider;

        public OpenMapInfoHandler(
            IViewLocator viewLocator,
            IClipboardProvider clipboardProvider)
        {
            this.viewLocator = viewLocator;
            this.clipboardProvider = clipboardProvider;
        }

        public async Task<bool> Handle(OpenMapInfoCommand request, CancellationToken cancellationToken)
        {
            var itemText = await clipboardProvider.Copy();

            await viewLocator.Open(View.Map, itemText);

            return true;
        }
    }
}
