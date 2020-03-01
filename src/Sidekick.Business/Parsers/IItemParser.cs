using System.Threading.Tasks;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.Business.Parsers
{
    public interface IItemParser
    {
        Task<Item> ParseItem(string itemText, bool parseAttributes);
    }
}
