using System.Drawing;

namespace Sidekick.Domain.Platforms
{
    /// <summary>
    /// Service providing screen functions
    /// </summary>
    public interface IScreenProvider
    {
        /// <summary>
        /// Initialize the provider
        /// </summary>
        void Initialize();

        /// <summary>
        /// Event that indicates a scroll down input occured
        /// </summary>
        Rectangle GetBounds();
    }
}
