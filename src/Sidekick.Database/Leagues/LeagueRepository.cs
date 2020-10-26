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
            await Clear();
            context.AddRange(leagues);
            await context.SaveChangesAsync();
        }

        public async Task Clear()
        {
            context.Leagues.RemoveRange(await FindAll());
        }
    }
}
