using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Game.Chat.Commands;
using Sidekick.Domain.Settings;

namespace Sidekick.Business.Parties
{
    public class PartyService : IPartyService
    {
        private readonly ILogger logger;
        private readonly ISidekickSettings settings;
        private readonly IMediator mediator;

        public PartyService(
            ILogger<PartyService> logger,
            ISidekickSettings settings,
            IMediator mediator)
        {
            this.logger = logger;
            this.settings = settings;
            this.mediator = mediator;
        }

        public async Task LeaveParty()
        {
            // This operation is only valid if the user has added their character name to the settings file.
            if (string.IsNullOrEmpty(settings.Character_Name))
            {
                logger.LogWarning(@"This command requires a ""CharacterName"" to be specified in the settings menu.");
                return;
            }
            await mediator.Send(new WriteChatCommand($"/kick {settings.Character_Name}"));
        }
    }
}
