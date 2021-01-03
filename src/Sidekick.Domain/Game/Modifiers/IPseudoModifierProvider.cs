using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Domain.Game.Modifiers.Models;

namespace Sidekick.Domain.Game.Items.Modifiers
{
    public interface IPseudoModifierProvider
    {
        Task Initialize();
        List<Modifier> Parse(ItemModifiers modifiers);
    }
}
