using System.Threading.Tasks;
using Sidekick.Domain.Game.Items;
using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Domain.Game.Modifiers
{
    public interface IModifierProvider
    {
        Task Initialize();
        ItemModifiers Parse(ParsingItem parsingItem);
        bool IsMatch(string id, string text);
    }
}
