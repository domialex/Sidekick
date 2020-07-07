using System;
using System.ComponentModel;
using Sidekick.Core.Logging;
using Sidekick.Helpers;

namespace Sidekick.Views.ApplicationLogs
{
    public class ApplicationLogViewModel : IDisposable, INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        private readonly SidekickEventSink eventSink;
        private bool isDisposed;

        public ApplicationLogViewModel(SidekickEventSink eventSink)
        {
            this.eventSink = eventSink;
            Logs = new ObservableList<string>(eventSink.Events);
            eventSink.LogEventEmitted += EventSink_LogEventEmitted;
        }

        private void EventSink_LogEventEmitted(string logEvent)
        {
            Logs.Add(logEvent);

            // Limit the log size to show.
            for (var i = Logs.Count; i > 100; i--)
            {
                Logs.RemoveAt(0);
            }
        }

        public ObservableList<string> Logs { get; private set; }

        public string Text { get; private set; }

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
                eventSink.LogEventEmitted -= EventSink_LogEventEmitted;
            }

            isDisposed = true;
        }
    }
}
