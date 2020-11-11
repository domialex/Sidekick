using System.Threading.Tasks;
using Sidekick.Domain.Views;

namespace Sidekick.Persistence.Views
{
    public class ViewPreferenceRepository : IViewPreferenceRepository
    {
        private readonly SidekickContext context;

        public ViewPreferenceRepository(SidekickContext context)
        {
            this.context = context;
        }

        public async Task<ViewPreference> Get(View id)
        {
            return await context.ViewPreferences.FindAsync(id);
        }

        public async Task SaveSize(View id, double width, double height)
        {
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
