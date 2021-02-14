namespace Sidekick.Domain.Platforms
{
    /// <summary>
    /// Service providing keybind functions
    /// </summary>
    public interface IKeybindProvider
    {
        /// <summary>
        /// Register keybinds with the operating system
        /// </summary>
        void Register();

        /// <summary>
        /// Unregisters keybinds with the operating system
        /// </summary>
        void Unregister();
    }
}
