using System.Threading.Tasks;

namespace Sidekick.Common.Blazor.Views
{
    /// <summary>
    /// Interface to manage views
    /// </summary>
    public interface IViewLocator
    {
        /// <summary>
        /// Opens the specified view
        /// </summary>
        /// <param name="view">The view to open and show</param>
        /// <param name="args">Arguments to pass to the view</param>
        Task Open(View view, params object[] args);

        /// <summary>
        /// Close the specified view
        /// </summary>
        /// <param name="view">The view to close</param>
        void Close(View view);

        /// <summary>
        /// Close all views
        /// </summary>
        /// <param name="view">The view to close</param>
        void CloseAll();

        /// <summary>
        /// Check if a view of the specified type is opened
        /// </summary>
        /// <param name="view">THe view to check if it is opened</param>
        /// <returns>true if a view is opened. false otherwise</returns>
        bool IsOpened(View view);

        /// <summary>
        /// Check if a view is opened
        /// </summary>
        /// <returns>true if a view is opened. false otherwise</returns>
        bool IsAnyOpened();
    }
}
