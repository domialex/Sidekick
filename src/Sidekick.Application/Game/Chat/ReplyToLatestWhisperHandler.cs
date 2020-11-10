using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Game.GameLogs.Queries;

namespace Sidekick.Application.Game.Chat
{
    public class ReplyToLatestWhisperHandler : ICommandHandler<LeavePartyCommand, bool>
    {
        private readonly IMediator mediator;

        public ReplyToLatestWhisperHandler(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Handle(LeavePartyCommand request, CancellationToken cancellationToken)
        {
            var characterName = await mediator.Send(new GetLatestWhisperCharacterNameQuery());
            if (!string.IsNullOrEmpty(characterName))
            {
                await mediator.Send(new StartWritingChatCommand($"@{characterName} "));
                return true;
            }
            return false;
        }
    }
}
