using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Business.Apis;
using Sidekick.Business.Apis.Poe.Parser;
using Sidekick.Domain.Clipboard;
using Sidekick.Domain.Wikis.Commands;

namespace Sidekick.Application.Wikis
{
    public class OpenWikiPageHandler : ICommandHandler<OpenWikiPageCommand, bool>
    {
        private readonly IClipboardProvider clipboardProvider;
        private readonly IParserService parserService;
        private readonly IWikiProvider wikiProvider;

        public OpenWikiPageHandler(
            IClipboardProvider clipboardProvider,
            IParserService parserService,
            IWikiProvider wikiProvider)
        {
            this.clipboardProvider = clipboardProvider;
            this.parserService = parserService;
            this.wikiProvider = wikiProvider;
        }

        public async Task<bool> Handle(OpenWikiPageCommand request, CancellationToken cancellationToken)
        {
            var text = await clipboardProvider.Copy();
            var item = parserService.ParseItem(text);

            if (item != null)
            {
                await wikiProvider.Open(item);
                return true;
            }

            return false;
        }
    }
}
