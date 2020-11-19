using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Sidekick.Domain.Apis.PoePriceInfo.Commands;
using Sidekick.Domain.Apis.PoePriceInfo.Models;
using Sidekick.Domain.Settings;
using Sidekick.Infrastructure.PoePriceInfo.Models;

namespace Sidekick.Infrastructure.PoePriceInfo
{
    public class PredictPriceHandler : IQueryHandler<PredictPriceCommand, PricePrediction>
    {
        private readonly IPoePriceInfoClient client;
        private readonly ISidekickSettings settings;
        private readonly IMapper mapper;

        public PredictPriceHandler(
            IPoePriceInfoClient client,
            ISidekickSettings settings,
            IMapper mapper)
        {
            this.client = client;
            this.settings = settings;
            this.mapper = mapper;
        }

        public async Task<PricePrediction> Handle(PredictPriceCommand request, CancellationToken cancellationToken)
        {
            var encodedItem = Convert.ToBase64String(Encoding.UTF8.GetBytes(request.Item.Text));
            var response = await client.Client.GetAsync("?l=" + settings.LeagueId + "&i=" + encodedItem);
            var content = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<PriceInfoResult>(content, client.Options);
            return mapper.Map<PricePrediction>(result);
        }
    }
}
