namespace Sidekick.Domain.Platforms
{
    /// <summary>
    /// Key down event when a key is pushed down
    /// </summary>
    public class KeyDownArgs
    {
        /// <summary>
        /// Key down event when a key is pushed down
        /// </summary>
        /// <param name="key">The key or key combination that was pressed</param>
        /// <param name="intercepted">Indicates if the input was intercepted by Sidekick</param>
        public KeyDownArgs(string key, bool intercepted)
        {
            Key = key;
            Intercepted = intercepted;
        }

        /// <summary>
        /// The key or key combination that was pressed
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// Indicates if the input was intercepted by Sidekick
        /// </summary>
        public bool Intercepted { get; }
    }
}
