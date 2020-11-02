using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Initialization.Notifications;
using Sidekick.Domain.Languages;

namespace Sidekick.Business.Apis.Poe.Trade.Data.Stats
{
    public class DataInitializationStartedHandler : INotificationHandler<DataInitializationStarted>
    {
        private readonly IPoeTradeClient poeApiClient;
        private readonly ILanguageProvider languageProvider;
        private readonly IStatDataService statDataService;
        private readonly ICacheRepository cacheRepository;

        public DataInitializationStartedHandler(IPoeTradeClient poeApiClient,
            ILanguageProvider languageProvider,
            IStatDataService statDataService,
            ICacheRepository cacheRepository)
        {
            this.poeApiClient = poeApiClient;
            this.languageProvider = languageProvider;
            this.statDataService = statDataService;
            this.cacheRepository = cacheRepository;
        }

        public async Task Handle(DataInitializationStarted notification, CancellationToken cancellationToken)
        {
            var categories = await cacheRepository.GetOrSet(
                "Sidekick.Business.Apis.Poe.Trade.Data.Stats.DataInitializationStartedHandler",
                () => poeApiClient.Fetch<StatDataCategory>());

            statDataService.PseudoPatterns = new List<StatData>();
            statDataService.ExplicitPatterns = new List<StatData>();
            statDataService.ImplicitPatterns = new List<StatData>();
            statDataService.EnchantPatterns = new List<StatData>();
            statDataService.CraftedPatterns = new List<StatData>();
            statDataService.VeiledPatterns = new List<StatData>();
            statDataService.FracturedPatterns = new List<StatData>();
            statDataService.IncreasedPattern = new Regex(languageProvider.Language.ModifierIncreased);

            var hashPattern = new Regex("\\\\#");
            var parenthesesPattern = new Regex("((?:\\\\\\ )*\\\\\\([^\\(\\)]*\\\\\\))");

            var additionalProjectileEscaped = Regex.Escape(languageProvider.Language.AdditionalProjectile);
            var additionalProjectiles = hashPattern.Replace(Regex.Escape(languageProvider.Language.AdditionalProjectiles), "([-+\\d,\\.]+)");

            // We need to ignore the case here, there are some mistakes in the data of the game.
            statDataService.AdditionalProjectilePattern = new Regex(languageProvider.Language.AdditionalProjectile, RegexOptions.IgnoreCase);

            foreach (var category in categories)
            {
                var first = category.Entries.FirstOrDefault();
                if (first == null)
                {
                    continue;
                }

                // The notes in parentheses are never translated by the game.
                // We should be fine hardcoding them this way.
                string suffix, pattern;
                List<StatData> patterns;
                switch (first.Id.Split('.').First())
                {
                    default: continue;
                    case "pseudo": suffix = "(?:\\n|(?<!(?:\\n.*){2,})$)"; patterns = statDataService.PseudoPatterns; break;
                    case "delve":
                    case "monster":
                    case "explicit": suffix = "(?:\\n|(?<!(?:\\n.*){2,})$)"; patterns = statDataService.ExplicitPatterns; break;
                    case "implicit": suffix = "(?:\\ \\(implicit\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = statDataService.ImplicitPatterns; break;
                    case "enchant": suffix = "(?:\\ \\(enchant\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = statDataService.EnchantPatterns; break;
                    case "crafted": suffix = "(?:\\ \\(crafted\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = statDataService.CraftedPatterns; break;
                    case "veiled": suffix = "(?:\\ \\(veiled\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = statDataService.VeiledPatterns; break;
                    case "fractured": suffix = "(?:\\ \\(fractured\\)\\n|(?<!(?:\\n.*){2,})$)"; patterns = statDataService.FracturedPatterns; break;
                }

                foreach (var entry in category.Entries)
                {
                    entry.Category = category.Label;

                    pattern = Regex.Escape(entry.Text);
                    pattern = parenthesesPattern.Replace(pattern, "(?:$1)?");
                    pattern = statDataService.NewLinePattern.Replace(pattern, "\\n");

                    if (entry.Option == null || entry.Option.Options == null || entry.Option.Options.Count == 0)
                    {
                        pattern = hashPattern.Replace(pattern, "([-+\\d,\\.]+)");
                    }
                    else
                    {
                        foreach (var option in entry.Option.Options)
                        {
                            if (statDataService.NewLinePattern.IsMatch(option.Text))
                            {
                                var lines = statDataService.NewLinePattern.Split(option.Text).ToList();
                                var options = lines.ConvertAll(x => hashPattern.Replace(pattern, Regex.Escape(x)));
                                option.Pattern = new Regex($"(?:^|\\n){string.Join("\\n", options)}{suffix}");
                                option.Text = string.Join("\n", lines.Select((x, i) => new
                                {
                                    Text = x,
                                    Index = i
                                })
                                .ToList()
                                .ConvertAll(x =>
                                {
                                    if (x.Index == 0)
                                    {
                                        return x.Text;
                                    }

                                    return entry.Text.Replace("#", x.Text);
                                }));
                            }
                            else
                            {
                                option.Pattern = new Regex($"(?:^|\\n){hashPattern.Replace(pattern, Regex.Escape(option.Text))}{suffix}", RegexOptions.None);
                            }
                        }

                        pattern = hashPattern.Replace(pattern, "(.*)");
                    }

                    if (statDataService.IncreasedPattern.IsMatch(pattern))
                    {
                        var negativePattern = statDataService.IncreasedPattern.Replace(pattern, languageProvider.Language.ModifierReduced);
                        entry.NegativePattern = new Regex($"(?:^|\\n){negativePattern}{suffix}", RegexOptions.None);
                    }

                    if (statDataService.AdditionalProjectilePattern.IsMatch(entry.Text))
                    {
                        var additionalProjectilePattern = pattern.Replace(additionalProjectileEscaped, additionalProjectiles, System.StringComparison.OrdinalIgnoreCase);
                        entry.AdditionalProjectilePattern = new Regex($"(?:^|\\n){additionalProjectilePattern}{suffix}", RegexOptions.IgnoreCase);
                    }

                    entry.Pattern = new Regex($"(?:^|\\n){pattern}{suffix}", RegexOptions.None);
                    patterns.Add(entry);
                }
            }
        }
    }
}
