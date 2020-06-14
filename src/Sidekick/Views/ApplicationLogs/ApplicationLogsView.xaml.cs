using System;
using System.ComponentModel;

namespace Sidekick.Views.ApplicationLogs
{
    public partial class ApplicationLogsView : BaseWindow
    {
        private readonly ApplicationLogViewModel viewModel;

        public ApplicationLogsView(IServiceProvider serviceProvider, ApplicationLogViewModel viewModel)
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

        protected override void OnClosing(CancelEventArgs e)
        {
            if (IsClosing)
            {
                return;
            }
            viewModel.Logs.CollectionChanged -= LogsChanged;
            viewModel.PropertyChanged -= LogsChanged;
            base.OnClosing(e);
        }
    }
}
