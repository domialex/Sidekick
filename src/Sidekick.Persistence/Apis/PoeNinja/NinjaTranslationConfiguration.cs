using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidekick.Domain.Apis.PoeNinja.Models;

namespace Sidekick.Persistence.Apis.PoeNinja
{
    internal class NinjaTranslationConfiguration : IEntityTypeConfiguration<NinjaTranslation>
    {
        public void Configure(EntityTypeBuilder<NinjaTranslation> builder)
        {
            builder.ToTable("NinjaTranslations");

            builder.HasKey(b => b.Translation);
        }
    }
}
