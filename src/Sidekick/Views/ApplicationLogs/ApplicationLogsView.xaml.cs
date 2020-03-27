using System;
using System.ComponentModel;
using System.Windows;
using Sidekick.UI.ApplicationLogs;

namespace Sidekick.Views.ApplicationLogs
{
    public partial class ApplicationLogsView : BaseWindow
  {
    private readonly IApplicationLogViewModel viewModel;

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
