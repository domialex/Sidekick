namespace Sidekick.Core.Settings
{
    public static class Extensions
    {
        public static string ToKeybindString(this string source)
        {
            return source?
                .Replace(" ", "")
                .Replace("+", " + ")
                .Replace(",", ", ");
        }
    }
}
