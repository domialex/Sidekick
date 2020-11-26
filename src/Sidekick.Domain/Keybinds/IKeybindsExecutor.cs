namespace Sidekick.Domain.Keybinds
{
    /// <summary>
    /// Service providing keybind functions
    /// </summary>
    public interface IKeybindsExecutor
    {
        /// <summary>
        /// Initialize the executor
        /// </summary>
        void Initialize();

        /// <summary>
        /// Indicate if the executor should execute keybind commands.
        /// </summary>
        bool Enabled { get; set; }
    }
}
