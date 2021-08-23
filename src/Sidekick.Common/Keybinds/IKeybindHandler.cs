using System.Threading.Tasks;

namespace Sidekick.Domain.Keybinds
{
    /// <summary>
    /// Interface for keybind handlers
    /// </summary>
    public interface IKeybindHandler : IBaseKeybindHandler
    {
        /// <summary>
        /// Executes the valid keybind
        /// </summary>
        /// <returns></returns>
        Task Execute();
    }
}
