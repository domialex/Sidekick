using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Common.Blazor
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickModule(this IServiceCollection services, Assembly assembly)
        {
            if (assembly != Assembly.GetEntryAssembly())
            {
                SidekickGlobals.Assemblies.Add(assembly);
            }

            return services;
        }
    }
}
