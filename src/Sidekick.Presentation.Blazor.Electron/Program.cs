using System;
using System.IO;
using ElectronNET.API;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Sidekick.Application.Settings;

namespace Sidekick.Presentation.Blazor.Electron
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var sidekickPath = Environment.ExpandEnvironmentVariables("%AppData%\\sidekick");

            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile(Path.Combine(sidekickPath, SaveSettingsHandler.FileName), true, true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .UseElectron(args)
                        .UseStartup<Startup>();
                });
        }
    }
}
