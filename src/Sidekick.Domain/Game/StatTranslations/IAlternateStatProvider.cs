using System;
using System.Collections.Generic;
using System.Text;

namespace Sidekick.Domain.Game.StatTranslations
{
    public interface IAlternateStatProvider
    {
        List<StatTranslation> Translations { get; }

        List<Stat> GetAlternateStats(string text);
    }
}
