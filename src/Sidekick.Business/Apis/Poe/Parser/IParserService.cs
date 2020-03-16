using System.Threading.Tasks;
using Sidekick.Business.Trades.Results;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public interface IParserService
    {
        Task<ResultItem> ParseItem(string itemText);
    }
}