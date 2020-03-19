using System.Threading.Tasks;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public interface IParserService
    {
        Task<ParsedItem> ParseItem(string itemText);
    }
}
