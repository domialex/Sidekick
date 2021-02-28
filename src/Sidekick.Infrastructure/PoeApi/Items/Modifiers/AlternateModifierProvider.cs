using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers
{
    public class AlternateModifierProvider : IAlternateModifierProvider
    {
        public AlternateModifierProvider()
        {
            Translations = new List<ModifierTranslation>();
        }

        public async Task Initialize()
        {
            var assembly = GetType().GetTypeInfo().Assembly;
            using var stream = assembly.GetManifestResourceStream("Sidekick.Infrastructure.PoeApi.Items.Modifiers.stat_translations.min.json");

            Translations = await JsonSerializer.DeserializeAsync<List<ModifierTranslation>>(stream);

            foreach (var translation in Translations)
                CleanTranslation(translation);

            Translations = Translations.Where(x => x.Stats.Any(y => y.Conditions != null && y.Conditions.Count >= 1)).ToList();
        }

        public List<ModifierTranslation> Translations { get; private set; }

        public List<Translation> GetAlternateStats(string text)
        {
            var alternates = new List<Translation>();
            var stats = Translations.Where(x => x.Stats.Any(y => y.Text == text)).SelectMany(z => z.Stats).Distinct().ToList();

            foreach (var stat in stats)
            {
                if (stat.Text != text && !alternates.Any(x => x.Text == stat.Text))
                    alternates.Add(stat);
            }

            return alternates;
        }

        private void CleanTranslation(ModifierTranslation translation)
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
                        var formatValue = stat.Formats[Int32.Parse(group.Value)];
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
