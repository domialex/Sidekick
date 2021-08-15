using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Common.Platform.Clipboard;
using Sidekick.Common.Platform.Windows.Keyboards;
using Sidekick.Common.Platform.Windows.Processes;
using Sidekick.Common.Platforms.Localization;

namespace Sidekick.Common.Platform
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickCommonPlatform(this IServiceCollection services)
        {
            services.AddTransient<PlatformResources>();
            services.AddTransient<IClipboardProvider, ClipboardProvider>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                services.AddSingleton<IProcessProvider, ProcessProvider>();
                services.AddSingleton<IKeyboardProvider, KeyboardProvider>();
            }

            return services;
        }
    }
}
