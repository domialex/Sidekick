using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Platforms;
using Sidekick.Platform.Clipboard;
using Sidekick.Platform.Keybinds;
using Sidekick.Platform.Processes;
using Sidekick.Platform.Scroll;

namespace Sidekick.Platform
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPlatform(this IServiceCollection services)
        {
            services.AddTransient<IClipboardProvider, ClipboardProvider>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddSingleton<IProcessProvider, ProcessProvider>();
                services.AddSingleton<IKeybindsProvider, KeybindsProvider>();
                services.AddSingleton<IScrollProvider, ScrollProvider>();
            }

            return services;
        }
    }
}
