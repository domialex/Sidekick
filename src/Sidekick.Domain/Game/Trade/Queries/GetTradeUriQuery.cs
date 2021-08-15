using System;
using MediatR;
using Sidekick.Common.Game.Items;

namespace Sidekick.Domain.Game.Trade.Queries
{
    /// <summary>
    /// Gets the Uri for the specified trade query id
    /// </summary>
    public class GetTradeUriQuery : IQuery<Uri>
    {
        /// <summary>
        /// Gets the Uri for the specified trade query id
        /// </summary>
        /// <param name="item">The item for which the uri is for</param>
        /// <param name="queryId">The trade query id</param>
        public GetTradeUriQuery(Item item, string queryId)
        {
            Item = item;
            QueryId = queryId;
        }

        /// <summary>
        /// The item for which the uri is for
        /// </summary>
        public Item Item { get; }

        /// <summary>
        /// The trade query id
        /// </summary>
        public string QueryId { get; }
    }
}
