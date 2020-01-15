namespace Sidekick.Windows.ApplicationLogs
{
    public static class ApplicationLogsController
    {
        private static ApplicationLogsWindow _applicationLogsWindow;
        public static void Show()
        {
            if (_applicationLogsWindow == null)
            {
                _applicationLogsWindow = new ApplicationLogsWindow();
            }

            _applicationLogsWindow.Activate();
            _applicationLogsWindow.OnWindowClosed += (s, e) => _applicationLogsWindow = null;
        }
    }
}
