using Sidekick.Business.Categories;
using Sidekick.Business.Filters;
using Sidekick.Business.Languages.Implementations;
using Sidekick.Business.Parsers.Models;
using Sidekick.Business.Parsers.Types;
using System.Collections.Generic;
using System.Linq;

namespace Sidekick.Business.Trades.Requests
{
    public class BulkQueryRequest
    {
        public BulkQueryRequest(Item item, ILanguage language, IStaticItemCategoryService staticItemCategoryService)
        {
            var itemType = item.GetType();

            if (itemType == typeof(CurrencyItem))
            {
                var itemCategory = "Currency";

                if (item.Name.Contains(language.KeywordCatalyst))
                {
                    itemCategory = "Catalysts";
                }
                else if (item.Name.Contains(language.KeywordOil))
                {
                    itemCategory = "Oils";
                }
                else if (item.Name.Contains(language.KeywordIncubator))
                {
                    itemCategory = "Incubators";
                }
                else if (item.Name.Contains(language.KeywordScarab))
                {
                    itemCategory = "Scarabs";
                }
                else if (item.Name.Contains(language.KeywordResonator))
                {
                    itemCategory = "DelveResonators";
                }
                else if (item.Name.Contains(language.KeywordFossil))
                {
                    itemCategory = "DelveFossils";
                }
                else if (item.Name.Contains(language.KeywordVial))
                {
                    itemCategory = "Vials";
                }
                else if (item.Name.Contains(language.KeywordEssence))
                {
                    itemCategory = "Essences";
                }

                var itemId = staticItemCategoryService.Categories.Single(x => x.Id == itemCategory)
                                                               .Entries
                                                               .Single(x => x.Text == item.Name)
                                                               .Id;

                Exchange.Want.Add(itemId);
                Exchange.Have.Add("chaos"); // TODO: Add support for other currency types?
            }
            else if (itemType == typeof(DivinationCardItem))
            {
                var itemId = staticItemCategoryService.Categories.Where(c => c.Id == "Cards").FirstOrDefault().Entries.Where(c => c.Text == item.Name).FirstOrDefault().Id;
                Exchange.Want.Add(itemId);
                Exchange.Have.Add("chaos");
            }
        }

        public Exchange Exchange { get; set; } = new Exchange();
        public Dictionary<string, SortType> Sort { get; set; } = new Dictionary<string, SortType> { { "price", SortType.Asc } };
    }
}
