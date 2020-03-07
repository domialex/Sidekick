using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sidekick.Business.Chat;
using Sidekick.Core.Settings;

namespace Sidekick.Business.Parties
{
    public class PartyService : IPartyService
    {
        private readonly ILogger logger;
        private readonly SidekickSettings settings;
        private readonly IChatService chatService;

        public PartyService(
            ILogger logger,
            SidekickSettings settings,
            IChatService chatService)
        {
            this.logger = logger;
            this.settings = settings;
            this.chatService = chatService;
        }

        public async Task LeaveParty()
        {
            // This operation is only valid if the user has added their character name to the settings file.
            if (string.IsNullOrEmpty(settings.Character_Name))
            {
                logger.LogWarning(@"This command requires a ""CharacterName"" to be specified in the settings menu.");
                return;
            }
            await chatService.Write($"/kick {settings.Character_Name}");
        }
    }
}
