using Microsoft.EntityFrameworkCore;
using Sidekick.Apis.PoeNinja.Repository.Models;

namespace Sidekick.Apis.PoeNinja.Repository
{
    public class PoeNinjaContext : DbContext
    {
        public PoeNinjaContext(DbContextOptions<PoeNinjaContext> options)
            : base(options)
        {
        }


        public DbSet<NinjaPrice> Prices { get; set; }

        public DbSet<NinjaTranslation> Translations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<NinjaPrice>()
                .HasKey(x => new
                {
                    x.Name,
                    x.Corrupted,
                    x.MapTier,
                    x.GemLevel,
                });
        }
    }
}
