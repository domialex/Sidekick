using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Sidekick.Windows.ApplicationLogs
{
    public partial class ApplicationLogsWindow : Window
    {
        public event EventHandler OnWindowClosed;
        public ApplicationLogsWindow()
        {
            InitializeComponent();
            logsTextBox.Text = GenerateLogLines;
            Legacy.Logger.MessageLogged += MessageLogged;
            logsScrollViewer.ScrollToEnd();
            Show();
        }

        public void SetText(string text)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new SetTextCallback(SetText), new object[] { text });
            }
            else
            {
                logsTextBox.Text = text;
            }
        }
        delegate void SetTextCallback(string text);

        public void ScrollToEnd()
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.Invoke(new ScrollToEndCallback(ScrollToEnd));
            }
            else
            {
                logsScrollViewer.ScrollToEnd();
            }
        }
        delegate void ScrollToEndCallback();

        private void MessageLogged()
        {
            SetText(GenerateLogLines);

            if (logsScrollViewer.VerticalOffset == logsScrollViewer.ScrollableHeight)
            {
                ScrollToEnd();
            }
        }

        private string GenerateLogLines => string.Join(Environment.NewLine, Legacy.Logger.Logs.Select(x => $"{x.Date.ToString()} - {x.Message}"));

        protected override void OnClosing(CancelEventArgs e)
        {
            OnWindowClosed?.Invoke(this, null);
            base.OnClosing(e);
        }
    }
}
