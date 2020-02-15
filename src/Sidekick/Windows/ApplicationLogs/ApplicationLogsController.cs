using Sidekick.Core.Loggers;

namespace Sidekick.Windows.ApplicationLogs
{
    public class ApplicationLogsController
    {
        private ApplicationLogsWindow applicationLogsWindow;

        public ApplicationLogsController(ILogger logger)
        {
            applicationLogsWindow = new ApplicationLogsWindow(logger);
        }
        public void Show()
        {
            applicationLogsWindow.Activate();
            applicationLogsWindow.OnWindowClosed += (s, e) => applicationLogsWindow = null;
        }
    }
}
