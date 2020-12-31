using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Process;

namespace Sidekick.Application.Game.Chat
{
    public class CustomChatHandler : ICommandHandler<CustomChatCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly INativeProcess nativeProcess;

        public CustomChatHandler(
            IMediator mediator,
            INativeProcess nativeProcess)
        {
            this.mediator = mediator;
            this.nativeProcess = nativeProcess;
        }

        public async Task<bool> Handle(CustomChatCommand request, CancellationToken cancellationToken)
        {
            if (!nativeProcess.IsPathOfExileInFocus)
            {
                return false;
            }

            await mediator.Send(new WriteChatCommand(request.Command));
            return true;
        }
    }
}
