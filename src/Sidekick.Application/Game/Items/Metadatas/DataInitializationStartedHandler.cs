using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Initialization.Notifications;

namespace Sidekick.Application.Game.Items.Metadatas
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IItemMetadataProvider itemMetadataProvider;
        private readonly IItemStaticDataProvider itemStaticDataProvider;

        public DataInitializationStartedHandler(
            IItemMetadataProvider itemMetadataProvider,
            IItemStaticDataProvider itemStaticDataProvider)
        {
            this.itemMetadataProvider = itemMetadataProvider;
            this.itemStaticDataProvider = itemStaticDataProvider;
        }

        public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            await itemMetadataProvider.Initialize();
            await itemStaticDataProvider.Initialize();
        }
    }
}
