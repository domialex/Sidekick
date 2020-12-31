using System.Text.RegularExpressions;

namespace Sidekick.Application.Game.Items.Parser.Extensions
{
    public static class ParserExtensions
    {
        private const string NEW_LINE = "[\\r\\n]";

        public static Regex ToRegex(this string input, string prefix = null, string suffix = null)
        {
            return new Regex($"{prefix}{Regex.Escape(input)}{suffix}");
        }

        public static Regex IntFromLineRegex(this string input) => input.ToRegex(prefix: NEW_LINE, suffix: "[^\\r\\n\\d]*(\\d+)");

        public static Regex DecimalFromLineRegex(this string input) => input.ToRegex(prefix: NEW_LINE, suffix: "[^\\r\\n\\d]*([\\d,\\.]+)");

        public static Regex RangeFromLineRegex(this string input) => input.ToRegex(prefix: NEW_LINE, suffix: "[^\\r\\n\\d]*(\\d+-\\d+)");

        public static Regex StartOfLineRegex(this string input) => input.ToRegex(prefix: "^");

        public static Regex EndOfLineRegex(this string input) => input.ToRegex(suffix: "$");

        public static Regex LineRegex(this string input) => input.ToRegex(prefix: NEW_LINE, suffix: "(?:.*)" + NEW_LINE);
    }
}
