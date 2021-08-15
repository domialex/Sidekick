using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Sidekick.Common.Extensions;

namespace Sidekick.Persistence
{
    internal class SidekickDesignTime : IDesignTimeDbContextFactory<SidekickContext>
    {
        public SidekickContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SidekickContext>();
            var connectionString = "Filename=" + SidekickPaths.GetDataFilePath("data.db");
            builder.UseSqlite(connectionString);
            return new SidekickContext(builder.Options);
        }
    }
}
