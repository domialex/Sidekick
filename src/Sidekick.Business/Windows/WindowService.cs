using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Database;
using Sidekick.Database.Windows;

namespace Sidekick.Business.Windows
{
    public class WindowService : IWindowService
    {
        private readonly DbContextOptions<SidekickContext> options;

        public WindowService(DbContextOptions<SidekickContext> options)
        {
            this.options = options;
        }

        public async Task<Window> Get(string id)
        {
            using var dbContext = new SidekickContext(options);

            return await dbContext.Windows.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task SaveSize(string id, double width, double height)
        {
            using var dbContext = new SidekickContext(options);

            var window = await dbContext.Windows.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (window == null)
            {
                window = new Window()
                {
                    Id = id,
                };
                dbContext.Windows.Add(window);
            }

            window.Height = height;
            window.Width = width;

            await dbContext.SaveChangesAsync();
        }
    }
}
