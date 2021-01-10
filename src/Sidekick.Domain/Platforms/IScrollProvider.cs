using System;

namespace Sidekick.Domain.Platforms
{
    /// <summary>
    /// Service providing keybind functions
    /// </summary>
    public interface IScrollProvider
    {
        /// <summary>
        /// Initialize the provider
        /// </summary>
        void Initialize();

        /// <summary>
        /// Event that indicates a scroll down input occured
        /// </summary>
        event Func<bool> OnScrollDown;

        /// <summary>
        /// Event that indicates a scroll up input occured
        /// </summary>
        event Func<bool> OnScrollUp;
    }
}
