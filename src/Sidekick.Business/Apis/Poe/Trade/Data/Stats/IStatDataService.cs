using System.Collections.Generic;
using System.Text.RegularExpressions;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public interface IStatDataService
    {
        List<StatData> PseudoPatterns { get; set; }
        List<StatData> ExplicitPatterns { get; set; }
        List<StatData> ImplicitPatterns { get; set; }
        List<StatData> EnchantPatterns { get; set; }
        List<StatData> CraftedPatterns { get; set; }
        List<StatData> VeiledPatterns { get; set; }
        List<StatData> FracturedPatterns { get; set; }

        Regex NewLinePattern { get; set; }
        Regex IncreasedPattern { get; set; }
        Regex AdditionalProjectilePattern { get; set; }

        Modifiers ParseMods(string text);
        StatData GetById(string id);
    }
}
