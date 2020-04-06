using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Sidekick.Business.Apis.Poe.Parser
{
    public static class ParserExtensions
    {
        private const string NEW_LINE = "[\\r\\n]";

        public static Regex ToCompiledRegex(this string input, string prefix = null, string suffix = null)
        {
            return new Regex($"{prefix}{Regex.Escape(input)}{suffix}");
        }

        public static Regex ToIntFromLineRegex(this string input) => input.ToCompiledRegex(prefix: NEW_LINE, suffix: "[^\\r\\n\\d]*(\\d+)");

        public static Regex ToDecimalFromLineRegex(this string input) => input.ToCompiledRegex(prefix: NEW_LINE, suffix: "[^\\r\\n\\d]*([\\d,\\.]+)");

        public static Regex ToRangeFromLineRegex(this string input) => input.ToCompiledRegex(prefix: NEW_LINE, suffix: "[^\\r\\n\\d]*(\\d+-\\d+)");

        public static Regex ToStartOfLineRegex(this string input) => input.ToCompiledRegex(prefix: "^");

        public static Regex ToEndOfLineRegex(this string input) => input.ToCompiledRegex(suffix: "$");
    }
}
