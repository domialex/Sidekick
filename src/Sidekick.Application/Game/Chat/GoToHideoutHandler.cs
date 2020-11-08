using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Process;

namespace Sidekick.Application.Game.Chat
{
    public class GoToHideoutHandler : ICommandHandler<GoToHideoutCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly INativeProcess nativeProcess;

        public GoToHideoutHandler(
            IMediator mediator,
            INativeProcess nativeProcess)
        {
            this.mediator = mediator;
            this.nativeProcess = nativeProcess;
        }

        public async Task<bool> Handle(GoToHideoutCommand request, CancellationToken cancellationToken)
        {
            if (!nativeProcess.IsPathOfExileInFocus)
            {
                return false;
            }

            await mediator.Send(new WriteChatCommand("/hideout"));
            return true;
        }
    }
}
