using System;
using Microsoft.Extensions.Logging;

namespace Sidekick.Core.Loggers
{
    public class Log
    {
        public DateTimeOffset Date { get; set; }
        public string Message { get; set; }
        public LogLevel Level { get; set; }
    }
}
