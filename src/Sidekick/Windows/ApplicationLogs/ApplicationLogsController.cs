using Sidekick.Core.Loggers;

namespace Sidekick.Windows.ApplicationLogs
{
    public class ApplicationLogsController
    {
        private readonly ILogger logger;
        private ApplicationLogsWindow applicationLogsWindow;

        public ApplicationLogsController(ILogger logger)
        {
            this.logger = logger;
        }

        public void Show()
        {
            if (applicationLogsWindow == null)
                applicationLogsWindow = new ApplicationLogsWindow(logger);

            applicationLogsWindow.Activate();
            applicationLogsWindow.Show();
            applicationLogsWindow.OnWindowClosed += (s, e) => applicationLogsWindow = null;
        }
    }
}
