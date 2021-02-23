using Microsoft.AspNetCore.Builder;

namespace Sidekick.Presentation.Blazor.Electron.App
{
    public static class ElectronCookieProtectionExtension
    {
        public static IApplicationBuilder AddElectronCookieProtection(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ElectronCookieProtectionMiddleware>();
        }
    }
}
