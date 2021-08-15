using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sidekick.Common;

namespace Sidekick.Apis.PoeNinja.Repository
{
    internal class PoeNinjaDesignTime : IDesignTimeDbContextFactory<PoeNinjaContext>
    {
        public PoeNinjaContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<PoeNinjaContext>();
            var connectionString = "Filename=" + SidekickPaths.GetDataFilePath("poeninja.db");
            builder.UseSqlite(connectionString);
            return new PoeNinjaContext(builder.Options);
        }
    }
}
