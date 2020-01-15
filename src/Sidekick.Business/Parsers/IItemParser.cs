using Sidekick.Business.Parsers.Models;

namespace Sidekick.Business.Parsers
{
    public interface IItemParser
    {
        Item ParseItem(string text);
    }
}