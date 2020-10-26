using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidekick.Domain.Leagues;

namespace Sidekick.Database.Leagues
{
    internal class LeagueConfiguration : IEntityTypeConfiguration<League>
    {
        public void Configure(EntityTypeBuilder<League> builder)
        {
            builder.ToTable("Leagues");

            builder.HasKey(b => b.Id);
        }
    }
}
