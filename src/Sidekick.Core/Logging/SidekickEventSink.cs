using System;
using System.Collections.Concurrent;
using System.IO;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;

namespace Sidekick.Core.Logging
{
    public class SidekickEventSink : ILogEventSink
    {
        private readonly ITextFormatter textFormatter = new MessageTemplateTextFormatter("{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}");

        public ConcurrentQueue<string> Events { get; } = new ConcurrentQueue<string>();
        public event Action<string> LogEventEmitted;

        public void Emit(LogEvent logEvent)
        {
            _ = logEvent ?? throw new ArgumentNullException(nameof(logEvent));
            var writer = new StringWriter();
            textFormatter.Format(logEvent, writer);

            var logMessage = writer.ToString();
            LogEventEmitted?.Invoke(logMessage);
            Events.Enqueue(logMessage);
        }
    }
}
