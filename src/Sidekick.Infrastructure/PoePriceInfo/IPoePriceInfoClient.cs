using System.Threading.Tasks;
using Sidekick.Infrastructure.PoePriceInfo.Models;

namespace Sidekick.Infrastructure.PoePriceInfo
{
    public interface IPoePriceInfoClient
    {
        Task<PriceInfoResult> GetItemPricePrediction(string itemText);
    }
}
