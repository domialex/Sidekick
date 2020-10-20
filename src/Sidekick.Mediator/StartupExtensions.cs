using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Mediator
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            services
                .AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddMediatR(
                    (config) => config.Using<SidekickMediator>().AsTransient(),
                    assemblies
                );

            return services;
        }
    }
}
