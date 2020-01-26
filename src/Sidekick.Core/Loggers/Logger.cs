using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sidekick.Core.Settings;
using Sidekick.Core.Initialization;

namespace Sidekick.Core.Loggers
{
    public class Logger : ILogger, IOnBeforeInit, IOnInit, IOnAfterInit, IOnReset
    {
        private readonly Settings.SidekickSettings configuration;

        public Logger(Settings.SidekickSettings configuration)
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
            Log($"Sidekick is ready, press {configuration.KeyPriceCheck.ToKeybindString()} over an item in-game to use. Press {configuration.KeyCloseWindow.ToKeybindString()} to close overlay.");
            return Task.CompletedTask;
        }

        public Task OnReset()
        {
            Log("Sidekick reset.");
            return Task.CompletedTask;
        }
    }
}
