using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;

namespace Sidekick.Application.Game.Chat
{
    public class CustomChatHandler : ICommandHandler<CustomChatCommand, bool>
    {
        private readonly IMediator mediator;

        public CustomChatHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Handle(CustomChatCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new WriteChatCommand(request.Command));
            return true;
        }
    }
}
