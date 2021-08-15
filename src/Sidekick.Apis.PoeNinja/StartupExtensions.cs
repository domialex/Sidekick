using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sidekick.Apis.PoeNinja.Localization;
using Sidekick.Apis.PoeNinja.Repository;
using Sidekick.Common;

namespace Sidekick.Apis.PoeNinja
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddSidekickPoeNinjaApi(this IServiceCollection services)
        {
            var connectionString = "Filename=" + SidekickPaths.GetDataFilePath("poeninja.db");

            services.AddDbContextPool<PoeNinjaContext>(options => options.UseSqlite(connectionString));

            var builder = new DbContextOptionsBuilder<PoeNinjaContext>();
            builder.UseSqlite(connectionString);
            var context = new PoeNinjaContext(builder.Options);
            context.Database.Migrate();

            services.AddTransient<IPoeNinjaRepository, PoeNinjaRepository>();
            services.AddTransient<PoeNinjaResources>();

            return services;
        }
    }
}
