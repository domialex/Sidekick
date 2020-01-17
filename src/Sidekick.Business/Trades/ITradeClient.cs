using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sidekick.Business.Trades.Models;
using Sidekick.Business.Trades.Results;

namespace Sidekick.Business.Trades
{
    public interface ITradeClient
    {
        JsonSerializerSettings JsonSerializerSettings { get; }
        League SelectedLeague { get; set; }
        List<League> Leagues { get; }
        event EventHandler LeaguesChanged;
        List<StaticItemCategory> StaticItemCategories { get; }
        List<AttributeCategory> AttributeCategories { get; }
        List<ItemCategory> ItemCategories { get; }
        HashSet<string> MapNames { get; }
        bool IsReady { get; }
        Task<bool> Initialize();
        Task FetchAPIData();
        Task<QueryResult<ListingResult>> GetListings(Parsers.Models.Item item);
        Task<QueryResult<ListingResult>> GetListings(QueryResult<string> queryResult, int page = 0);
        Task<QueryResult<ListingResult>> GetListingsForSubsequentPages(Parsers.Models.Item item, int nextPageToFetch);
        Task<QueryResult<string>> Query(Parsers.Models.Item item);
        Task OpenWebpage(Parsers.Models.Item item);
        void Dispose();
    }
}
