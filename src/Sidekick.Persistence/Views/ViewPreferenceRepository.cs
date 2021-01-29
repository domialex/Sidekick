using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Domain.Views;

namespace Sidekick.Persistence.Views
{
    public class ViewPreferenceRepository : IViewPreferenceRepository
    {
        private readonly DbContextOptions<SidekickContext> options;

        public ViewPreferenceRepository(DbContextOptions<SidekickContext> options)
        {
            this.options = options;
        }

        public async Task<ViewPreference> Get(View id)
        {
            using var context = new SidekickContext(options);
            return await context.ViewPreferences.FindAsync(id);
        }

        public async Task SaveSize(View id, int width, int height)
        {
            using var context = new SidekickContext(options);
            var preference = await context.ViewPreferences.FindAsync(id);

            if (preference == null)
            {
                preference = new ViewPreference()
                {
                    Id = id,
                };
                context.ViewPreferences.Add(preference);
            }

            preference.Height = height;
            preference.Width = width;

            await context.SaveChangesAsync();
        }
    }
}
