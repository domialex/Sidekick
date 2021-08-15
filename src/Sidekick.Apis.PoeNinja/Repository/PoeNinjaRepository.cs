using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sidekick.Apis.PoeNinja.Repository.Models;
using Sidekick.Common.Game.Items;

namespace Sidekick.Apis.PoeNinja.Repository
{
    public class PoeNinjaRepository : IPoeNinjaRepository
    {
        private readonly DbContextOptions<PoeNinjaContext> options;

        public PoeNinjaRepository(DbContextOptions<PoeNinjaContext> options)
        {
            this.options = options;
        }

        public async Task<NinjaPrice> Find(Item item)
        {
            using var dbContext = new PoeNinjaContext(options);

            var name = item.Original.Name;
            var type = item.Original.Type;

            var translation = await dbContext.Translations.FirstOrDefaultAsync(x => x.Translation == name);
            if (translation != null)
            {
                name = translation.English;
            }

            var query = dbContext.Prices
                .Where(x => x.Name == name || x.Name == type);

            if (item.Properties != null)
            {
                query = query.Where(x => x.Corrupted == item.Properties.Corrupted && x.MapTier == item.Properties.MapTier && x.GemLevel == item.Properties.GemLevel);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task SavePrices(List<NinjaPrice> prices)
        {
            using var dbContext = new PoeNinjaContext(options);

            prices = prices
                .GroupBy(x => (x.Name, x.Corrupted, x.MapTier, x.GemLevel))
                .Select(x => x.OrderBy(x => x.Price).First())
                .ToList();

            var names = prices.Select(x => x.Name);
            var data = await dbContext.Prices
                .Where(x => names.Any(y => y == x.Name))
                .ToListAsync();

            dbContext.Prices.RemoveRange(data);
            await dbContext.SaveChangesAsync();

            dbContext.Prices.AddRange(prices);
            await dbContext.SaveChangesAsync();
        }

        public async Task SaveTranslations(List<NinjaTranslation> translations)
        {
            using var dbContext = new PoeNinjaContext(options);

            translations = translations
                .GroupBy(x => x.English)
                .Select(x => x.First())
                .ToList();

            var names = translations.Select(x => x.English);
            var data = await dbContext.Translations
                .Where(x => names.Any(y => y == x.English))
                .ToListAsync();

            dbContext.Translations.RemoveRange(data);
            await dbContext.SaveChangesAsync();

            dbContext.Translations.AddRange(translations);
            await dbContext.SaveChangesAsync();
        }

        public async Task Clear()
        {
            using var dbContext = new PoeNinjaContext(options);

            var prices = await dbContext.Prices.ToListAsync();
            dbContext.Prices.RemoveRange(prices);

            var translations = await dbContext.Translations.ToListAsync();
            dbContext.Translations.RemoveRange(translations);

            await dbContext.SaveChangesAsync();
        }
    }
}
