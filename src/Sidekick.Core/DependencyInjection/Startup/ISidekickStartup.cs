using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Core.DependencyInjection
{
  public interface ISidekickStartup
  {
    void ConfigureServices(IServiceCollection services, IConfiguration configuration);
  }
}
