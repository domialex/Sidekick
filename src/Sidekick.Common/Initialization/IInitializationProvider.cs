using System.Threading.Tasks;

namespace Sidekick.Common.Initialization
{
    public interface IInitializationProvider
    {
        /// <summary>
        /// Notification to indicate that the initialization has progressed
        /// </summary>
        /// <param name="percentage">The current initialization percentage</param>
        /// <param name="title">The title of the current initialization progress</param>
        Task OnProgress(int percentage, string title = null);
    }
}
