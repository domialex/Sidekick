using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Domain.Cache;
using Sidekick.Domain.Leagues;
using Sidekick.Domain.Views;
using Sidekick.Persistence.Cache;
using Sidekick.Persistence.ItemCategories;
using Sidekick.Persistence.Leagues;
using Sidekick.Persistence.Views;

namespace Sidekick.Persistence
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPersistence(this IServiceCollection services)
        {
            services.AddDbContextPool<SidekickContext>(options => options.UseSqlite("Filename=Sidekick_database.db"));

            var builder = new DbContextOptionsBuilder<SidekickContext>();
            builder.UseSqlite("Filename=Sidekick_database.db");
            var context = new SidekickContext(builder.Options);
            context.Database.Migrate();

            services.AddTransient<ICacheRepository, CacheRepository>();
            services.AddSingleton<IItemCategoryRepository, ItemCategoryRepository>();
            services.AddTransient<ILeagueRepository, LeagueRepository>();
            services.AddTransient<IViewPreferenceRepository, ViewPreferenceRepository>();

            return services;
        }
    }
}
