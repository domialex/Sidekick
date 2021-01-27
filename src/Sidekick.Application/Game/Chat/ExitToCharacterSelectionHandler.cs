using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;

namespace Sidekick.Application.Game.Chat
{
    public class ExitToCharacterSelectionHandler : ICommandHandler<ExitToCharacterSelectionCommand, bool>
    {
        private readonly IMediator mediator;

        public ExitToCharacterSelectionHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Handle(ExitToCharacterSelectionCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new WriteChatCommand("/exit"));
            return true;
        }
    }
}
