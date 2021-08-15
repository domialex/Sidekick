using System.Threading.Tasks;

namespace Sidekick.Common.Initialization
{
    public interface IInitializationProvider
    {
        /// <summary>
        /// Command to initialize the application
        /// </summary>
        /// <param name="firstRun">Indicates if this command is called at the start of the application, or after some setting changes</param>
        /// <param name="autoUpdate">Indicates if we should auto update</param>
        Task Initialize(bool firstRun, bool autoUpdate);

        /// <summary>
        /// Notification to indicate that the initialization has progressed
        /// </summary>
        /// <param name="percentage">The current initialization percentage</param>
        /// <param name="title">The title of the current initialization progress</param>
        Task OnProgress(int percentage, string title = null);
    }
}
