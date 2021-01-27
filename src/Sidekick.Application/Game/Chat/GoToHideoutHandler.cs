using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;

namespace Sidekick.Application.Game.Chat
{
    public class GoToHideoutHandler : ICommandHandler<GoToHideoutCommand, bool>
    {
        private readonly IMediator mediator;

        public GoToHideoutHandler(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Handle(GoToHideoutCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new WriteChatCommand("/hideout"));
            return true;
        }
    }
}
