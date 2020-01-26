namespace Sidekick.Core.Configuration
{
    public static class ConfigurationExtensions
    {
        public static string ToKeybindString(this string source)
        {
            return source
                .Replace(" ", "")
                .Replace("+", " + ")
                .Replace(",", ", ");
        }
    }
}
