using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Extensions;
using System.Runtime.Serialization;

namespace Sidekick.Core.DependencyInjection.Startup
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddSidekickStartup(this IServiceCollection services, IConfiguration configuration)
        {
            foreach (var type in typeof(ISidekickStartup).GetImplementedInterface())
            {
                var instance = (ISidekickStartup)FormatterServices.GetUninitializedObject(type);
                instance.ConfigureServices(services, configuration);
            }

            return services;
        }
    }
}
