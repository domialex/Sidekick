using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Game.Items.Commands;
using Sidekick.Domain.Wikis.Commands;

namespace Sidekick.Application.Wikis
{
    public class OpenWikiPageHandler : ICommandHandler<OpenWikiPageCommand, bool>
    {
        private readonly IClipboardProvider clipboardProvider;
        private readonly IWikiProvider wikiProvider;
        private readonly IMediator mediator;

        public OpenWikiPageHandler(
            IClipboardProvider clipboardProvider,
            IWikiProvider wikiProvider,
            IMediator mediator)
        {
            this.clipboardProvider = clipboardProvider;
            this.wikiProvider = wikiProvider;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(OpenWikiPageCommand request, CancellationToken cancellationToken)
        {
            var text = await clipboardProvider.Copy();
            var item = await mediator.Send(new ParseItemCommand(text));

            if (item != null)
            {
                await wikiProvider.Open(item);
                return true;
            }

            return false;
        }
    }
}
