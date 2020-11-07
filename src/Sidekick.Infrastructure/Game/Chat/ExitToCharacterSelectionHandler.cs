using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Process;

namespace Sidekick.Infrastructure.Game.Chat
{
    public class ExitToCharacterSelectionHandler : ICommandHandler<ExitToCharacterSelectionCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly INativeProcess nativeProcess;

        public ExitToCharacterSelectionHandler(
            IMediator mediator,
            INativeProcess nativeProcess)
        {
            this.mediator = mediator;
            this.nativeProcess = nativeProcess;
        }

        public async Task<bool> Handle(ExitToCharacterSelectionCommand request, CancellationToken cancellationToken)
        {
            if (!nativeProcess.IsPathOfExileInFocus)
            {
                return false;
            }

            await mediator.Send(new WriteChatCommand("/exit"));
            return true;
        }
    }
}
