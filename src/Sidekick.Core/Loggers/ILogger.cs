using System;
using System.Collections.Generic;

namespace Sidekick.Core.Loggers
{
    public interface ILogger
    {
        List<Log> Logs { get; }

        event EventHandler MessageLogged;

        void Clear();
        void Log(string text, LogState state = LogState.None);
        void LogException(Exception e);
    }
}
