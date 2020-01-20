namespace Sidekick.Helpers.Input
{
    /// <summary>
    /// Predefined SendKey commands used to send input to the Path of Exile application
    /// </summary>
    public static class KeyCommands
    {
        public const string HIDEOUT = "{Enter}/hideout{Enter}{Enter}{Up}{Up}{Esc}";
        public const string STASH_LEFT = "{Left}";
        public const string STASH_RIGHT = "{Right}";
        public const string COPY = "^{c}";

        public const string FIND_ITEMS = "^{f}^{a}^{v}{Enter}";

        public const string LEAVE_PARTY = "{Enter}/kick {name}{Enter}"; // replaces {name} in send so keep that as is
    }
}