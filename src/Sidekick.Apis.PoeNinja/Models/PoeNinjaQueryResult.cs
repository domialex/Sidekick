using System.Collections.Generic;

namespace Sidekick.Apis.PoeNinja.Models
{
    public class PoeNinjaQueryResult<T>
    {
        public List<T> Lines { get; set; }
        public PoeNinjaQueryResultLanguage Language { get; set; }
    }
}
