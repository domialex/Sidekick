using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Game.GameLogs.Queries;

namespace Sidekick.Application.Game.Chat
{
    public class ReplyToLastWhisperHandler : ICommandHandler<ReplyToLastWhisperCommand, bool>
    {
        private readonly IMediator mediator;

        public ReplyToLastWhisperHandler(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<bool> Handle(ReplyToLastWhisperCommand request, CancellationToken cancellationToken)
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
