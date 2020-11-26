using System.Collections.Generic;

namespace Sidekick.Infrastructure.PoeNinja.Models
{
    public class PoeNinjaQueryResultLanguage
    {
        /// <summary>
        /// Language code.
        /// </summary>
        public string Name { get; set; }
        public Dictionary<string, string> Translations { get; set; }
    }
}
