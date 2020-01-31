using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;

namespace Sidekick.Core.Loggers
{
    public class Logger : ILogger, IOnBeforeInit, IOnInit, IOnAfterInit, IOnReset
    {
        private readonly SidekickSettings configuration;

        public Logger(SidekickSettings configuration)
        {
            this.configuration = configuration;
        }

        public event Action MessageLogged;
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
            MessageLogged?.Invoke();
        }

        public void LogException(Exception e)
        {
            var log = new Log()
            {
                Date = DateTime.Now,
                Message = $"EXCEPTION! {e.Message} | {e.StackTrace}",
                State = LogState.Error
            };

            if (Logs.Count >= 100)
            {
                Logs.RemoveAt(0);
            }

            Logs.Add(log);
        }

        public void Clear()
        {
            Logs.Clear();
        }

        public Task OnBeforeInit()
        {
            Log("Sidekick before initialization.");
            return Task.CompletedTask;
        }

        public Task OnInit()
        {
            Log("Sidekick initialization.");
            return Task.CompletedTask;
        }

        public Task OnAfterInit()
        {
            Log("Sidekick after initialization.");
            Log($"Sidekick is ready, press {configuration.Key_CheckPrices.ToKeybindString()} over an item in-game to use. Press {configuration.Key_CloseWindow.ToKeybindString()} to close overlay.");
            return Task.CompletedTask;
        }

        public Task OnReset()
        {
            Log("Sidekick reset.");
            return Task.CompletedTask;
        }
    }
}
