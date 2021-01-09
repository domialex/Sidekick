using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers
{
    public interface IAlternateModifierProvider
    {
        List<ModifierTranslation> Translations { get; }

        Task Initialize();

        List<Translation> GetAlternateStats(string text);
    }
}
