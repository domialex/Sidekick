using System.Threading.Tasks;
using Sidekick.Apis.Poe.Parser;
using Sidekick.Common.Game.Items.Modifiers;

namespace Sidekick.Apis.Poe.Modifiers
{
    public interface IModifierProvider
    {
        Task Initialize();

        ItemModifiers Parse(ParsingItem parsingItem);

        bool IsMatch(string id, string text);
    }
}
