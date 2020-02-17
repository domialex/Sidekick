using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Sidekick.Core.Loggers;

namespace Sidekick.UI.ApplicationLogs
{
    public class ApplicationLogViewModel : IApplicationLogViewModel, IDisposable
    {
        private readonly ILogger logger;

        public event PropertyChangedEventHandler PropertyChanged;

        public ApplicationLogViewModel(ILogger logger)
        {
            this.logger = logger;
            Logs = new ObservableCollection<Log>(logger.Logs);
            logger.MessageLogged += Logger_MessageLogged;
        }

        public ObservableCollection<Log> Logs { get; private set; }

        public string Text { get; private set; }

        private void Logger_MessageLogged(Log log)
        {
            Logs.Add(log);

            // Limit the log size to show.
            for (var i = Logs.Count; i > 100; i--)
            {
                Logs.RemoveAt(0);
            }
        }

        public void Dispose()
        {
            logger.MessageLogged -= Logger_MessageLogged;
        }
    }
}
