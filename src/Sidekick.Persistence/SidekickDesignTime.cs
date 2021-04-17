using System;
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
            var sidekickPath = Environment.ExpandEnvironmentVariables("%AppData%\\sidekick");

            var builder = new DbContextOptionsBuilder<SidekickContext>();
            var connectionString = "Filename=" + Path.Combine(sidekickPath, "data.db");
            builder.UseSqlite(connectionString);
            return new SidekickContext(builder.Options);
        }
    }
}
