using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Sidekick.Database
{
    public static class StartupExtensions
    {
        private static bool Added = false;

        public static IServiceCollection AddSidekickDatabase(this IServiceCollection services)
        {
            if (Added) { return services; }
            Added = true;

            services.AddDbContextPool<SidekickContext>(options => options.UseSqlite("Filename=Sidekick.db"));

            return services;
        }
    }
}
