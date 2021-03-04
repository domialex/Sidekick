using System.Text.RegularExpressions;

namespace Sidekick.Application.Game.Items.Parser.Patterns
{
    public static class RegexExtensions
    {
        public static Regex ToRegexIntCapture(this string input) => new Regex($"^{Regex.Escape(input)}[^\\d]*(\\d+)");

        public static Regex ToRegexDecimalCapture(this string input) => new Regex($"^{Regex.Escape(input)}[^\\d]*([\\d,\\.]+)");

        public static Regex ToRegexStartOfLine(this string input) => new Regex($"^{Regex.Escape(input)}.*$");

        public static Regex ToRegexEndOfLine(this string input) => new Regex($"^.*{Regex.Escape(input)}$");

        public static Regex ToRegexLine(this string input) => new Regex($"^{Regex.Escape(input)}$");
    }
}
