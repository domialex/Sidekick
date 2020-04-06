using System.Threading.Tasks;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public interface IParserService
    {
        Task<Item> ParseItem(string itemText);
    }
}
