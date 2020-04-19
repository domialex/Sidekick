using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Static
{
    public interface IStaticDataService
    {
        string GetImage(string id);
        string GetId(string text);
        string GetId(Item item);
    }
}
