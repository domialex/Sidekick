using Microsoft.EntityFrameworkCore;
using Sidekick.Database.Caches;
using Sidekick.Database.Windows;

namespace Sidekick.Database
{
    public class SidekickContext : DbContext
    {
        public SidekickContext(DbContextOptions<SidekickContext> options)
            : base(options)
        {
        }

        public DbSet<Cache> Caches { get; set; }

        public DbSet<Window> Windows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("sidekick");
        }
    }
}
