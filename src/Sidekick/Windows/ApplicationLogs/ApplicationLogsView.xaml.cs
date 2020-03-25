using System;
using System.Windows;
using Sidekick.UI.ApplicationLogs;

namespace Sidekick.Windows.ApplicationLogs
{
    public partial class ApplicationLogsView : BaseWindow, IDisposable
    {
        private readonly IApplicationLogViewModel viewModel;
        private bool isDisposed;

        public ApplicationLogsView(
            IApplicationLogViewModel viewModel,
            IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            this.viewModel = viewModel;

            InitializeComponent();
            DataContext = this;

            viewModel.PropertyChanged += LogsChanged;
            viewModel.Logs.CollectionChanged += LogsChanged;

            Show();
            GenerateLogLines();
        }

        private void LogsChanged(object sender, EventArgs e)
        {
            Dispatcher.Invoke(GenerateLogLines);
        }

        private void GenerateLogLines()
        {
            if (logsTextBox != null)
            {
                logsTextBox.Text = string.Concat(viewModel.Logs);
                logsScrollViewer.ScrollToEnd();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed)
            {
                return;
            }

            if (disposing)
            {
                viewModel.Logs.CollectionChanged -= LogsChanged;
                viewModel.PropertyChanged -= LogsChanged;
            }

            isDisposed = true;
        }
    }
}
