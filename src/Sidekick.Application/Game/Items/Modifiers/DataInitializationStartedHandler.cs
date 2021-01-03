using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Modifiers;
using Sidekick.Domain.Game.Modifiers;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Application.Game.Items.Modifiers
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IModifierProvider modifierProvider;
        private readonly IPseudoModifierProvider pseudoModifierProvider;

        public DataInitializationStartedHandler(
            IModifierProvider modifierProvider,
            IPseudoModifierProvider pseudoModifierProvider)
        {
            this.modifierProvider = modifierProvider;
            this.pseudoModifierProvider = pseudoModifierProvider;
        }

        public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            await modifierProvider.Initialize();
            await pseudoModifierProvider.Initialize();
        }
    }
}
