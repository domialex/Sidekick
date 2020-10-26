using Microsoft.EntityFrameworkCore;
using Sidekick.Database.Caches;
using Sidekick.Database.ItemCategories;
using Sidekick.Database.Leagues;
using Sidekick.Database.Windows;
using Sidekick.Domain.Leagues;

namespace Sidekick.Database
{
    public class SidekickContext : DbContext
    {
        public SidekickContext(DbContextOptions<SidekickContext> options)
            : base(options)
        {
        }

        public DbSet<Cache> Caches { get; set; }

        public DbSet<ItemCategory> ItemCategories { get; set; }

        public DbSet<League> Leagues { get; set; }

        public DbSet<Window> Windows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("sidekick");
            modelBuilder.ApplyConfiguration(new LeagueConfiguration());
        }
    }
}
