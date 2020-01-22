using System.Threading.Tasks;

namespace Sidekick.Business.Apis.PoePriceInfo.Models
{
    public interface IPoePriceInfoClient
    {
        Task<PriceInfoResult> GetItemPricePrediction(string itemText);
    }
}
