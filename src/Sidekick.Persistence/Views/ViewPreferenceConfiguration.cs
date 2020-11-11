using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sidekick.Domain.Views;

namespace Sidekick.Persistence.Views
{
    internal class ViewPreferenceConfiguration : IEntityTypeConfiguration<ViewPreference>
    {
        public void Configure(EntityTypeBuilder<ViewPreference> builder)
        {
            builder.ToTable("ViewPreferences");

            builder.HasKey(b => b.Id);
        }
    }
}
