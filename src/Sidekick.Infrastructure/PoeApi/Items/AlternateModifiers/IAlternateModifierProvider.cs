using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models.Alternate;

namespace Sidekick.Infrastructure.PoeApi.Items.AlternateModifiers
{
    public interface IAlternateModifierProvider
    {
        Task Initialize();

        List<ModifierPattern> GetAlternateModifiers(string text);
    }
}
