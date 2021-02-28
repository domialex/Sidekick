using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Game.Items.Metadatas;
using Sidekick.Domain.Game.Items.Models;
using Sidekick.Domain.Game.Languages;
using Sidekick.Domain.Game.Trade.Queries;
using Sidekick.Domain.Settings;

namespace Sidekick.Application.Game.Trade
{
    public class GetTradeUriHandler : IQueryHandler<GetTradeUriQuery, Uri>
    {
        private readonly IGameLanguageProvider gameLanguageProvider;
        private readonly IItemStaticDataProvider itemStaticDataProvider;
        private readonly ISidekickSettings settings;

        public GetTradeUriHandler(IGameLanguageProvider gameLanguageProvider,
            IItemStaticDataProvider itemStaticDataProvider,
            ISidekickSettings settings)
        {
            this.gameLanguageProvider = gameLanguageProvider;
            this.itemStaticDataProvider = itemStaticDataProvider;
            this.settings = settings;
        }

        public Task<Uri> Handle(GetTradeUriQuery request, CancellationToken cancellationToken)
        {
            Uri baseUri;

            if (request.Item.Rarity == Rarity.Currency && itemStaticDataProvider.GetId(request.Item) != null)
            {
                baseUri = gameLanguageProvider.Language.PoeTradeExchangeBaseUrl;
            }
            else
            {

                baseUri = gameLanguageProvider.Language.PoeTradeSearchBaseUrl;
            }

            return Task.FromResult(new Uri(baseUri, $"{settings.LeagueId}/{request.QueryId}"));
        }
    }
}
