using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Database.Cache;
using Sidekick.Database.Leagues;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Leagues;

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

            var builder = new DbContextOptionsBuilder<SidekickContext>();
            builder.UseSqlite("Filename=Sidekick.db");
            var context = new SidekickContext(builder.Options);
            context.Database.Migrate();

            services.AddTransient<ICacheRepository, CacheRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();

            return services;
        }
    }
}
