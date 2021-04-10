using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace Sidekick.Persistence
{
    internal class SidekickDesignTime : IDesignTimeDbContextFactory<SidekickContext>
    {
        public SidekickContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SidekickContext>();
            var connectionString = "Filename=" + Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "data.db");
            builder.UseSqlite(connectionString);
            return new SidekickContext(builder.Options);
        }
    }
}
