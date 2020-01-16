using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Extensions;
using System.Linq;

namespace Sidekick.Core.DependencyInjection.Services
{
    public static class BuilderExtensions
    {
        public static IServiceCollection AddSidekickServices(this IServiceCollection services)
        {
            var types = typeof(SidekickServiceAttribute)
              .GetImplementedAttribute()
              .Select(x => new
              {
                  Type = x,
                  Attribute = x.GetAttribute<SidekickServiceAttribute>()
              });

            foreach (var type in types)
            {
                switch (type.Attribute.Lifetime)
                {
                    case LifetimeEnum.Scoped:
                        services.AddScoped(type.Attribute.Interface, type.Type);
                        break;
                    case LifetimeEnum.Singleton:
                        services.AddSingleton(type.Attribute.Interface, type.Type);
                        break;
                    case LifetimeEnum.Transcient:
                        services.AddTransient(type.Attribute.Interface, type.Type);
                        break;
                }
            }

            return services;
        }
    }
}
