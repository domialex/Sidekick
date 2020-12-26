using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Domain.Apis.PoeNinja;
using Sidekick.Domain.Apis.PoeNinja.Models;

namespace Sidekick.Persistence.Apis.PoeNinja
{
    public class PoeNinjaRepository : IPoeNinjaRepository
    {
        private readonly DbContextOptions<SidekickContext> options;

        public PoeNinjaRepository(DbContextOptions<SidekickContext> options)
        {
            this.options = options;
        }

        public async Task<NinjaPrice> Find(string name, bool corrupted, int mapTier, int gemLevel)
        {
            using var dbContext = new SidekickContext(options);

            var translation = await dbContext.NinjaTranslations.FirstOrDefaultAsync(x => x.Translation == name);
            if (translation != null)
            {
                name = translation.English;
            }

            return await dbContext.NinjaPrices.FirstOrDefaultAsync(x => x.Name == name && x.Corrupted == corrupted && x.MapTier == mapTier && x.GemLevel == gemLevel);
        }

        public async Task SavePrices(List<NinjaPrice> prices)
        {
            using var dbContext = new SidekickContext(options);

            prices = prices
                .GroupBy(x => (x.Name, x.Corrupted, x.MapTier, x.GemLevel))
                .Select(x => x.OrderBy(x => x.Price).First())
                .ToList();

            var names = prices.Select(x => x.Name);
            var data = await dbContext.NinjaPrices
                .Where(x => names.Any(y => y == x.Name))
                .ToListAsync();

            dbContext.NinjaPrices.RemoveRange(data);
            await dbContext.SaveChangesAsync();

            dbContext.NinjaPrices.AddRange(prices);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveTranslations(List<NinjaTranslation> translations)
        {
            using var dbContext = new SidekickContext(options);

            translations = translations
                .GroupBy(x => x.English)
                .Select(x => x.First())
                .ToList();

            var names = translations.Select(x => x.English);
            var data = await dbContext.NinjaTranslations
                .Where(x => names.Any(y => y == x.English))
                .ToListAsync();

            dbContext.NinjaTranslations.RemoveRange(data);
            await dbContext.SaveChangesAsync();

            dbContext.NinjaTranslations.AddRange(translations);
            await dbContext.SaveChangesAsync();
        }

        public async Task Clear()
        {
            using var dbContext = new SidekickContext(options);

            var prices = await dbContext.NinjaPrices.ToListAsync();
            dbContext.NinjaPrices.RemoveRange(prices);

            var translations = await dbContext.NinjaTranslations.ToListAsync();
            dbContext.NinjaTranslations.RemoveRange(translations);

            await dbContext.SaveChangesAsync();
        }
    }
}
