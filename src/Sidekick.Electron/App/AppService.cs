using System;
using System.Threading.Tasks;
using ElectronNET.API.Entities;
using Microsoft.AspNetCore.Hosting;
using Sidekick.Common;

namespace Sidekick.Electron.App
{
    public class AppService : IAppService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public AppService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task OpenConfirmationNotification(string message, string title = null, Func<Task> onYes = null, Func<Task> onNo = null)
        {
            var options = new MessageBoxOptions(message)
            {
                Buttons = new[] { "Yes", "No" },
                CancelId = 1,
                DefaultId = 0,
                Title = title,
                Icon = $"{webHostEnvironment.ContentRootPath}Assets/icon.png"
            };

            var result = await ElectronNET.API.Electron.Dialog.ShowMessageBoxAsync(options);
            if (result.Response == 0)
            {
                await onYes.Invoke();
            }
            else
            {
                await onNo.Invoke();
            }
        }

        public Task OpenNotification(string message, string title = null)
        {
            var options = new NotificationOptions(title, message)
            {
                Icon = $"{webHostEnvironment.ContentRootPath}Assets/icon.png"
            };

            ElectronNET.API.Electron.Notification.Show(options);
            return Task.CompletedTask;
        }

        public void Shutdown()
        {
            ElectronNET.API.Electron.App.Exit();
        }
    }
}
