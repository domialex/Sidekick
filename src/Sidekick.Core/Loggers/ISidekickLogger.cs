using System;
using System.Collections.Generic;

namespace Sidekick.Core.Loggers
{
    public interface ISidekickLogger
    {
        event Action<Log> MessageLogged;
        List<Log> Logs { get; }
    }
}
