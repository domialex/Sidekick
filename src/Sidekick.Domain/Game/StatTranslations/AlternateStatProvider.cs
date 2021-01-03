using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Sidekick.Domain.Game.StatTranslations
{
    public class AlternateStatProvider : IAlternateStatProvider
    {
        public AlternateStatProvider()
        {
            Translations = new List<StatTranslation>();

            var assembly = this.GetType().GetTypeInfo().Assembly;
            using Stream resource = assembly.GetManifestResourceStream("Sidekick.Domain.Game.StatTranslations.stat_translations.min.json");

            using var stream = new StreamReader(resource);
            var data = stream.ReadToEnd();
            Translations = JsonSerializer.Deserialize<List<StatTranslation>>(data);

            foreach (var translation in Translations)
                CleanTranslation(translation);

            Translations = Translations.Where(x => x.Stats.Any(y => y.Conditions != null && y.Conditions.Count >= 1)).ToList();
        }

        public List<StatTranslation> Translations { get; private set; }

        public List<Stat> GetAlternateStats(string text)
        {
            var alternates = new List<Stat>();
            var stats = Translations.Where(x => x.Stats.Any(y => y.Text == text)).SelectMany(z => z.Stats).Distinct().ToList();

            foreach (var stat in stats)
            {
                if (stat.Text != text && !alternates.Any(x => x.Text == stat.Text))
                    alternates.Add(stat);
            }

            return alternates;
        }

        private void CleanTranslation(StatTranslation translation)
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
