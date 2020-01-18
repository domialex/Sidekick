using Sidekick.Core.DependencyInjection.Services;
using Sidekick.Core.Initialization;
using Sidekick.Core.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sidekick.Core.Loggers
{
    [SidekickService(typeof(ILogger))]
    public class Logger : ILogger, IOnBeforeInitialize, IOnInitialize, IOnAfterInitialize, IOnReset
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

        public Task OnBeforeInitialize()
        {
            Log("Sidekick before initialization.");
            return Task.CompletedTask;
        }

        public Task OnInitialize()
        {
            Log("Sidekick initialization.");
            return Task.CompletedTask;
        }

        public Task OnAfterInitialize()
        {
            Log("Sidekick after initialization.");
            Log($"Sidekick is ready, press {KeybindSetting.PriceCheck.GetTemplate()} over an item in-game to use. Press {KeybindSetting.CloseWindow.GetTemplate()} to close overlay.");
            return Task.CompletedTask;
        }

        public Task OnReset()
        {
            Log("Sidekick reset.");
            return Task.CompletedTask;
        }
    }
}
