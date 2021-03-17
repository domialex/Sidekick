using System.Threading.Tasks;

namespace Sidekick.Domain.Views
{
    /// <summary>
    /// Interface to manage a view instance
    /// </summary>
    public interface IViewInstance
    {
        /// <summary>
        /// Indicates if the view can be minimized
        /// </summary>
        bool Minimizable { get; }

        /// <summary>
        /// Minimizes the view
        /// </summary>
        Task Minimize();

        /// <summary>
        /// Indicates if the view can be maximized
        /// </summary>
        bool Maximizable { get; }

        /// <summary>
        /// Maximizes the view
        /// </summary>
        Task Maximize();

        /// <summary>
        /// Closes the view
        /// </summary>
        Task Close();
    }
}
