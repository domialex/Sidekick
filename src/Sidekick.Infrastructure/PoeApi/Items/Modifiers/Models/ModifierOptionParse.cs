using System.Text.RegularExpressions;

namespace Sidekick.Infrastructure.PoeApi.Items.Modifiers.Models
{
    public class ModifierOptionParse
    {
        public string Text { get; set; }

        public Regex Pattern { get; set; }
    }
}
