using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sidekick.Database.Cache
{
    internal class CacheConfiguration : IEntityTypeConfiguration<Cache>
    {
        public void Configure(EntityTypeBuilder<Cache> builder)
        {
            builder.ToTable("Caches");

            builder.HasKey(b => b.Key);
        }
    }
}
