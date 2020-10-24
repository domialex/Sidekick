using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Mediator.Internal;

namespace Sidekick.Mediator
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            services
                .AddSingleton(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .AddMediatR(
                    (config) => config.Using<Mediator>().AsSingleton(),
                    assemblies
                );

            services.AddSingleton<IMediator, Mediator>((sp) => (Mediator)sp.GetService<MediatR.IMediator>());

            return services;
        }
    }
}
