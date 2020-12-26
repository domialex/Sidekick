using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidekick.Domain.Apis.PoeNinja.Models;

namespace Sidekick.Persistence.Apis.PoeNinja
{
    internal class NinjaPriceConfiguration : IEntityTypeConfiguration<NinjaPrice>
    {
        public void Configure(EntityTypeBuilder<NinjaPrice> builder)
        {
            builder.ToTable("NinjaPrices");

            builder.HasKey(b => new
            {
                b.Name,
                b.Corrupted,
                b.MapTier,
                b.GemLevel,
            });
        }
    }
}
