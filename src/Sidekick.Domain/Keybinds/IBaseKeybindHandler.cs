namespace Sidekick.Domain.Keybinds
{
    /// <summary>
    /// Interface for keybind handlers
    /// </summary>
    public interface IBaseKeybindHandler
    {
        /// <summary>
        /// Check if this keybind should be executed
        /// </summary>
        /// <returns>true if we need to handle this keybind</returns>
        bool IsValid();
    }
}
