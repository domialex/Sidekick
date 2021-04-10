using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Infrastructure.RePoe.Data.StatTranslations.Models;

namespace Sidekick.Infrastructure.RePoe.Data.StatTranslations
{
    public interface IStatTranslationProvider
    {
        Task Initialize();

        List<AlternateModifier> GetAlternateModifiers(string text);
    }
}
