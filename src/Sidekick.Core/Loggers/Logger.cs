using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sidekick.Core.Initialization;

namespace Sidekick.Core.Loggers
{
    public class Logger : ILogger, ISidekickLogger, IDisposable
    {
        private readonly IInitializer initializer;

        public Logger(IInitializer initializer)
        {
            this.initializer = initializer;

            initializer.OnProgress += Initializer_OnProgress;
        }

        private void Initializer_OnProgress(ProgressEventArgs obj)
        {
            this.LogInformation($"{obj.TotalPercentage}% - {obj.Message} ({obj.ServiceName})");
        }

        public event Action<Log> MessageLogged;
        public List<Log> Logs { get; private set; } = new List<Log>();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message;
            if (formatter != null)
            {
                message = formatter(state, exception);
            }
            else
            {
                message = string.Empty;
            }

            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            lock (Logs)
            {
                var log = new Log()
                {
                    Date = DateTimeOffset.Now,
                    Message = message,
                    Level = logLevel,
                };

                if (Logs.Count >= 100)
                {
                    Logs.RemoveRange(0, Logs.Count - 100);
                }

                Logs.Add(log);
                MessageLogged?.Invoke(log);
            }
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public void Dispose()
        {
            initializer.OnProgress -= Initializer_OnProgress;
        }
    }
}
