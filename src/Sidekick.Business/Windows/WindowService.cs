using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Sidekick.Database;
using Sidekick.Database.Windows;

namespace Sidekick.Business.Windows
{
    public class WindowService : IWindowService
    {
        private readonly ILogger logger;
        private readonly DbContextOptions<SidekickContext> options;

        public WindowService(ILogger logger, DbContextOptions<SidekickContext> options)
        {
            this.logger = logger;
            this.options = options;
        }

        public async Task<Window> Get(string id)
        {
            using var dbContext = new SidekickContext(options);

            logger.Debug($"WindowService : Getting data for {id}");
            return await dbContext.Windows.FindAsync(id);
        }

        public async Task SaveSize(string id, double width, double height)
        {
            using var dbContext = new SidekickContext(options);

            var window = await dbContext.Windows.FindAsync(id);

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

            logger.Debug($"WindowService : Saved data for {id}. Width: {width}, Height: {height}");
        }
    }
}
