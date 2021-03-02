using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models.Alternate;

namespace Sidekick.Infrastructure.PoeApi.Items.AlternateModifiers
{
    public class AlternateModifierProvider : IAlternateModifierProvider
    {
        public AlternateModifierProvider()
        {
            Translations = new List<ModifierTranslation>();
        }

        public async Task Initialize()
        {
            using var stream = typeof(AlternateModifierProvider).Assembly.GetManifestResourceStream("Sidekick.Infrastructure.RePoe.Data.stat_translations.min.json");

            Translations = await JsonSerializer.DeserializeAsync<List<ModifierTranslation>>(stream);
            CleanTranslation();

            Translations = Translations.Where(x => x.Stats.Any(y => y.Conditions != null && y.Conditions.Count >= 1)).ToList();
        }

        private List<ModifierTranslation> Translations { get; set; }

        public List<ModifierPattern> GetAlternateModifiers(string text)
        {
            return new List<ModifierPattern>();
            /*
            var alternates = new List<Translation>();
            var stats = Translations.Where(x => x.Stats.Any(y => y.Text == text)).SelectMany(z => z.Stats).Distinct().ToList();

            foreach (var stat in stats)
            {
                if (stat.Text != text && !alternates.Any(x => x.Text == stat.Text))
                    alternates.Add(stat);
            }

            return alternates;*/
        }

        private void CleanTranslation()
        {
            foreach (var translation in Translations)
            {
                foreach (var stat in translation.Stats)
                {
                    foreach (var condition in stat.Conditions.ToList())
                    {
                        if (condition.Max == null && condition.Min == null)
                            stat.Conditions.Remove(condition);
                    }

                    stat.Text = Regex.Replace(stat.Text, @"{(\d)}", (match) =>
                    {
                        if (match.Groups.Count > 1)
                        {
                            var group = match.Groups[1];
                            var formatValue = stat.Formats[int.Parse(group.Value)];
                            if (formatValue == "ignore")
                                return "";

                            return formatValue;
                        }

                        return match.Value;
                    });

                    stat.Formats = null;
                }
            }
        }
    }
}
