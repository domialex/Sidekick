using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Sidekick.Domain.Apis.PoePriceInfo.Models;
using Sidekick.Domain.Apis.PoePriceInfo.Queries;
using Sidekick.Domain.Settings;
using Sidekick.Infrastructure.PoePriceInfo.Models;

namespace Sidekick.Infrastructure.PoePriceInfo
{
    public class GetPricePredictionHandler : IQueryHandler<GetPricePredictionQuery, PricePrediction>
    {
        private readonly IPoePriceInfoClient client;
        private readonly ISidekickSettings settings;
        private readonly IMapper mapper;
        private readonly ILogger logger;

        public GetPricePredictionHandler(
            IPoePriceInfoClient client,
            ISidekickSettings settings,
            IMapper mapper,
            ILogger<GetPricePredictionHandler> logger)
        {
            this.client = client;
            this.settings = settings;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<PricePrediction> Handle(GetPricePredictionQuery request, CancellationToken cancellationToken)
        {
            if (request.Item.Metadata.Rarity != Domain.Game.Items.Models.Rarity.Rare)
            {
                return null;
            }

            try
            {
                var encodedItem = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Item.Original.Text));
                var response = await client.Client.GetAsync("?l=" + settings.LeagueId + "&i=" + encodedItem, cancellationToken);
                var content = await response.Content.ReadAsStreamAsync();
                var result = await JsonSerializer.DeserializeAsync<PriceInfoResult>(content, client.Options);

                if (result.Min == 0 && result.Max == 0)
                {
                    return null;
                }

                return mapper.Map<PricePrediction>(result);
            }
            catch (Exception e)
            {
                logger.LogWarning(e, "Error while trying to get price prediction from poeprices.info.");
            }

            return null;
        }
    }
}
