using System;
using System.Reflection;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Mapper
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickMapper(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddAutoMapper(configAction =>
            {
                configAction.AddMaps(assemblies);
            });

            return services;
        }

        public static IServiceProvider UseSidekickMapper(this IServiceProvider serviceProvider)
        {
            var mapper = serviceProvider.GetService<IMapper>();
            // mapper.ConfigurationProvider.AssertConfigurationIsValid();

            return serviceProvider;
        }
    }
}
