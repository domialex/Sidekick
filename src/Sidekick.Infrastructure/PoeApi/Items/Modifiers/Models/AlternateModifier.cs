using System.Text.RegularExpressions;
using Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models.Alternate;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    public class AlternateModifier
    {
        public Translation Stat { get; set; }

        public Regex Pattern { get; set; }
    }
}
