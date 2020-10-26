using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Sidekick.Database.DesignTime
{
    internal class SidekickDesignTime : IDesignTimeDbContextFactory<SidekickContext>
    {
        public SidekickContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<SidekickContext>();
            builder.UseSqlite("Filename=Sidekick.db");
            return new SidekickContext(builder.Options);
        }
    }
}
