using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sidekick.Apis.Poe.Translations.Stats.Models;

namespace Sidekick.Apis.Poe.Translations.Stats
{
    public class StatTranslationProvider : IStatTranslationProvider
    {
        public async Task Initialize()
        {
            using var stream = typeof(StatTranslationProvider).Assembly.GetManifestResourceStream("Sidekick.Apis.Poe.Translations.Stats.stat_translations.min.json");

            var translations = await JsonSerializer.DeserializeAsync<List<Translation>>(stream);
            FeedAlternateModifiers(translations);
        }

        private void FeedAlternateModifiers(List<Translation> translations)
        {
            AlternateModifiers = new List<List<AlternateModifier>>();
            List<AlternateModifier> alternateModifiers;
            AlternateModifier alternateModifier;

            foreach (var translation in translations.Where(x => x.Stats.Count > 1))
            {
                alternateModifiers = new List<AlternateModifier>();

                foreach (var stat in translation.Stats)
                {
                    alternateModifier = new AlternateModifier()
                    {
                        Text = stat.Text,
                        Value = stat.Conditions.Where(x => x.Min == x.Max).Select(x => x.Min).FirstOrDefault(),
                        Negative = stat.Conditions.All(x => x.Max < 0),
                    };

                    alternateModifier.Text = Regex.Replace(alternateModifier.Text, @"{(\d)}", (match) =>
                    {
                        if (match.Success)
                        {
                            var formatValue = stat.Formats[int.Parse(match.Groups[1].Value)];
                            if (formatValue == "ignore") return "";
                            return formatValue;
                        }
                        return match.Value;
                    });

                    alternateModifiers.Add(alternateModifier);
                }

                AlternateModifiers.Add(alternateModifiers);
            }
        }

        private List<List<AlternateModifier>> AlternateModifiers { get; set; }

        public List<AlternateModifier> GetAlternateModifiers(string text)
        {
            return AlternateModifiers.FirstOrDefault(x => x.Any(y => y.Text == text));
        }
    }
}
