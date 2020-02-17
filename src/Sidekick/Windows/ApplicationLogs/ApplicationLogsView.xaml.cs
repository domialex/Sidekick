using System;
using System.Linq;
using System.Windows;
using Sidekick.UI.ApplicationLogs;

namespace Sidekick.Windows.ApplicationLogs
{
    public partial class ApplicationLogsView : BaseWindow, IDisposable
    {
        private readonly IApplicationLogViewModel viewModel;

        public ApplicationLogsView(IApplicationLogViewModel viewModel, IServiceProvider serviceProvider)
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
            GenerateLogLines();
        }

        private void GenerateLogLines()
        {
            if (logsTextBox != null)
            {
                logsTextBox.Text = string.Join(Environment.NewLine, viewModel.Logs.Select(x => $"{x.Date.ToString()} - {x.Message}"));
                logsScrollViewer.ScrollToEnd();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Dispose()
        {
            viewModel.Logs.CollectionChanged -= LogsChanged;
            viewModel.PropertyChanged -= LogsChanged;
        }
    }
}
