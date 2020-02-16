using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Sidekick.Core.Loggers;

namespace Sidekick.Windows.ApplicationLogs
{
    public partial class ApplicationLogsWindow : Window
    {
        private readonly ILogger logger;
        public event EventHandler OnWindowClosed;
        public ApplicationLogsWindow(ILogger logger)
        {
            this.logger = logger;
            InitializeComponent();
            logsTextBox.Text = GenerateLogLines;
            logger.MessageLogged += MessageLogged;
            logsScrollViewer.ScrollToEnd();
            Show();
            MouseLeftButtonDown += Window_MouseLeftButtonDown;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
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

        private string GenerateLogLines => string.Join(Environment.NewLine, logger.Logs.Select(x => $"{x.Date.ToString()} - {x.Message}"));

        protected override void OnClosing(CancelEventArgs e)
        {
            OnWindowClosed?.Invoke(this, null);
            base.OnClosing(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
