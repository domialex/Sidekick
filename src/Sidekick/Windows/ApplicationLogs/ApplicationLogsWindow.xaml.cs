using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

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

        private void MessageLogged(object sender, EventArgs e)
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
