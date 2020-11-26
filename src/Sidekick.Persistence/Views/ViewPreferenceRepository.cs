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
            return await context.ViewPreferences.FindAsync(id.ToString());
        }

        public async Task SaveSize(View id, double width, double height)
        {
            using var context = new SidekickContext(options);
            var preference = await context.ViewPreferences.FindAsync(id.ToString());

            if (preference == null)
            {
                preference = new ViewPreference()
                {
                    Id = id.ToString(),
                };
                context.ViewPreferences.Add(preference);
            }

            preference.Height = height;
            preference.Width = width;

            await context.SaveChangesAsync();
        }
    }
}
