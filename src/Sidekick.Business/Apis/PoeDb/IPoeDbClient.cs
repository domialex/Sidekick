using Sidekick.Business.Parsers.Models;

namespace Sidekick.Business.Apis.PoeDb
{
    public interface IPoeDbClient
    {
        void Open(Item item);
    }
}