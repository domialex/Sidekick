using System.Threading.Tasks;
using Sidekick.Business.Trades.Results;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public interface IParserService
    {
        Task<Item> ParseItem(string itemText);
    }
}