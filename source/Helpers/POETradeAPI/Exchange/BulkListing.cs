using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sidekick.Helpers.POETradeAPI.Exchange
{
    public class BulkListing
    {
        public CurrencyRatio Ratio { get; set; }
        public int Stock { get; set; }
        public string SellerAccountName { get; set; }
        public string SellerCharacterName { get; set; }
    }

    public class CurrencyRatio
    {
        public CurrencyRatio(double give, double receive)
        {
            SellerGives = give;
            BuyerReceives = receive;
            UnitPrice = BuyerReceives / SellerGives;
        }
        public double SellerGives { get; }
        public double BuyerReceives { get; }
        public double UnitPrice { get; }
    }
}
