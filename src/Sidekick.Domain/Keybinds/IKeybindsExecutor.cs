using System.Threading.Tasks;

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
        /// Executes a keybind
        /// </summary>
        Task<bool> Execute(string keybind);
    }
}
