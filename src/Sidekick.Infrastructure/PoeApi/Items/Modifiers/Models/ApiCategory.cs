using System.Collections.Generic;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    /// <summary>
    /// Pseudo, Explicit, Implicit, etc.
    /// </summary>
    public class ApiCategory
    {
        public string Label { get; set; }

        public List<ApiModifier> Entries { get; set; }

    }
}
