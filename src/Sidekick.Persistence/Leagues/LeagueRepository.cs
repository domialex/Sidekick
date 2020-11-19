using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Domain.Game.Leagues;

namespace Sidekick.Persistence.Leagues
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly DbContextOptions<SidekickContext> options;

        public LeagueRepository(DbContextOptions<SidekickContext> options)
        {
            this.options = options;
        }

        public async Task<List<League>> FindAll()
        {
            using var context = new SidekickContext(options);
            return await context.Leagues.ToListAsync();
        }

        public async Task SaveAll(List<League> leagues)
        {
            using var context = new SidekickContext(options);
            context.Leagues.RemoveRange(await FindAll());
            context.AddRange(leagues);
            await context.SaveChangesAsync();
        }
    }
}
