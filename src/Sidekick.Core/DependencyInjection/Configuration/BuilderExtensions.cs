using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Core.Extensions;
using System.Runtime.Serialization;

namespace Sidekick.Core.DependencyInjection.Configuration
{
  public static partial class BuilderExtensions
  {
    public static IServiceCollection AddSidekickConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
      foreach (var type in typeof(SidekickConfigAttribute).GetImplementedAttribute())
      {
        var attribute = type.GetAttribute<SidekickConfigAttribute>();
        if (attribute != null)
        {
          var config = FormatterServices.GetUninitializedObject(type);
          configuration.GetSection(attribute.Section).Bind(config);
          services.AddSingleton(type, config);
        }
      }

      return services;
    }
  }
}
