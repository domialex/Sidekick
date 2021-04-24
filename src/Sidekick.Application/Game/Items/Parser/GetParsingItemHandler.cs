using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Application.Game.Items.Parser.Tokenizers;
using Sidekick.Domain.Game.Items;
using Sidekick.Domain.Game.Items.Commands;

namespace Sidekick.Application.Game.Items.Parser
{
    public class GetParsingItemHandler : ICommandHandler<GetParsingItem, ParsingItem>
    {
        public GetParsingItemHandler()
        {
        }

        public Task<ParsingItem> Handle(GetParsingItem request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ItemText))
            {
                return Task.FromResult<ParsingItem>(null);
            }

            var itemText = new ItemNameTokenizer().CleanString(request.ItemText);
            return Task.FromResult(new ParsingItem(itemText));
        }
    }
}
