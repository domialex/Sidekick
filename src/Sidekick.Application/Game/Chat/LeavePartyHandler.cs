using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Settings;

namespace Sidekick.Application.Game.Chat
{
    public class LeavePartyHandler : ICommandHandler<LeavePartyCommand, bool>
    {
        private readonly ISidekickSettings settings;
        private readonly ILogger<LeavePartyHandler> logger;
        private readonly IMediator mediator;

        public LeavePartyHandler(
            ISidekickSettings settings,
            ILogger<LeavePartyHandler> logger,
            IMediator mediator)
        {
            this.settings = settings;
            this.logger = logger;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(LeavePartyCommand request, CancellationToken cancellationToken)
        {
            // This operation is only valid if the user has added their character name to the settings file.
            if (string.IsNullOrEmpty(settings.Character_Name))
            {
                logger.LogWarning(@"This command requires a ""CharacterName"" to be specified in the settings menu.");
                return false;
            }

            await mediator.Send(new WriteChatCommand($"/kick {settings.Character_Name}"));
            return true;
        }
    }
}
