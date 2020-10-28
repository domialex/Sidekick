using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Domain.Leagues;

namespace Sidekick.Database.Leagues
{
    public class LeagueRepository : ILeagueRepository
    {
        private readonly SidekickContext context;

        public LeagueRepository(SidekickContext context)
        {
            this.context = context;
        }

        public async Task<List<League>> FindAll()
        {
            return await context.Leagues.ToListAsync();
        }

        public async Task SaveAll(List<League> leagues)
        {
            context.Leagues.RemoveRange(await FindAll());
            context.AddRange(leagues);
            await context.SaveChangesAsync();
        }
    }
}
