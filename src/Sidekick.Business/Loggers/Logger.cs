using System;
using System.Collections.Generic;

namespace Sidekick.Business.Loggers
{
    public class Logger : ILogger
    {
        public event EventHandler MessageLogged;
        public List<Log> Logs { get; private set; } = new List<Log>();

        public void Log(string text, LogState state = LogState.None)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            var log = new Log()
            {
                Date = DateTime.Now,
                Message = text,
                State = state
            };

            if (Logs.Count >= 100)
            {
                Logs.RemoveAt(0);
            }

            Logs.Add(log);
            MessageLogged?.Invoke(null, null);
        }

        public void Clear()
        {
            Logs.Clear();
        }
    }
}
