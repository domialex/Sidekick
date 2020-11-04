using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Leagues;
using Sidekick.Persistence.Cache;
using Sidekick.Persistence.Leagues;

namespace Sidekick.Persistence
{
    public static class StartupExtensions
    {
        private static bool Added = false;

        public static IServiceCollection AddSidekickDatabase(this IServiceCollection services)
        {
            if (Added) { return services; }
            Added = true;

            services.AddDbContextPool<SidekickContext>(options => options.UseSqlite("Filename=Sidekick_database.db"));

            var builder = new DbContextOptionsBuilder<SidekickContext>();
            builder.UseSqlite("Filename=Sidekick_database.db");
            var context = new SidekickContext(builder.Options);
            context.Database.Migrate();

            services.AddTransient<ICacheRepository, CacheRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();

            return services;
        }
    }
}
