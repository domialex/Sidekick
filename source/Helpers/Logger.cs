using System;
using System.Collections.Generic;

namespace Sidekick.Helpers
{
    public static class Logger
    {
        public static event EventHandler MessageLogged;
        public static List<Log> Logs { get; private set; } = new List<Log>();

        public static void Log(string text, LogState state = LogState.None)
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
            if(Logs.Count >= 100)
            {
                Logs.RemoveAt(0);
            }
            Logs.Add(log);
            MessageLogged?.Invoke(null, null);
        }

        public static void Clear()
        {
            Logs.Clear();
        }
    }

    public class Log
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public LogState State { get; set; }
    }

    public enum LogState
    {
        None,
        Success,
        Error,
        Warning
    }
}
