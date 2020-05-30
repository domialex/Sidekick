using System.Collections.Generic;

namespace Sidekick.Business.Apis.PoeNinja.Models
{
    public class PoeNinjaQueryResult<T> where T : PoeNinjaResult
    {
        public List<T> Lines { get; set; }
        public PoeNinjaQueryResultLanguage Language { get; set; }
    }
}
