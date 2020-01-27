using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core;
using Sidekick.Core.Natives;
using Sidekick.Platforms.Windows.Keyboards;
using Sidekick.Platforms.Windows.Natives;

namespace Sidekick.Platforms.Windows
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickWindowsServices(this IServiceCollection services)
        {
            services.AddSingleton<INativeBrowser, NativeBrowser>();
            services.AddSingleton<INativeClipboard, NativeClipboard>();

            services.AddInitializableService<IKeybindEvents, KeybindEvents>();
            services.AddInitializableService<INativeKeyboard, NativeKeyboard>();
            services.AddInitializableService<INativeProcess, NativeProcess>();

            return services;
        }
    }
}
