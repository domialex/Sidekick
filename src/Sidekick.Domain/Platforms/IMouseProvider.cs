using System.Drawing;

namespace Sidekick.Domain.Platforms
{
    /// <summary>
    /// Service providing keybind functions
    /// </summary>
    public interface IMouseProvider
    {
        /// <summary>
        /// Initialize the provider
        /// </summary>
        void Initialize();

        /// <summary>
        /// Get the position of the cursor
        /// </summary>
        Point GetPosition();
    }
}
