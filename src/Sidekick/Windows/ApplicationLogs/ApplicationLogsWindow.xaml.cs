using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
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
            InitializeIcon();
            Show();
        }

        private static MemoryStream IconStream;

        private void InitializeIcon()
        {
            //TODO: Do better than this, ExaltedOrb Icon is referenced in code...
            if (IconStream == null)
            {
                lock (this)
                {
                    if (IconStream == null)
                    {
                        var ms = new MemoryStream();
                        Sidekick.Resources.ExaltedOrb.Save(ms);
                        IconStream = ms;
                    }
                }
            }

            IconStream.Seek(0, SeekOrigin.Begin);
            var frame = BitmapFrame.Create(IconStream);
            Icon = frame;
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

        private string GenerateLogLines => string.Join(Environment.NewLine,logger.Logs.Select(x => $"{x.Date.ToString()} - {x.Message}"));

        protected override void OnClosing(CancelEventArgs e)
        {
            OnWindowClosed?.Invoke(this, null);
            base.OnClosing(e);
        }
    }
}
