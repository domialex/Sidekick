using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Apis.Poe.Translations.Stats.Models;

namespace Sidekick.Apis.Poe.Translations.Stats
{
    public interface IStatTranslationProvider
    {
        Task Initialize();

        List<AlternateModifier> GetAlternateModifiers(string text);
    }
}
