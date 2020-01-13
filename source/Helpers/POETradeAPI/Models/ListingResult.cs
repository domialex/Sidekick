using System;

namespace Sidekick.Helpers.POETradeAPI.Models
{
    public class ListingResult
    {
        public string Id { get; set; }
        public Listing Listing { get; set; }
        public ItemListing Item { get; set; }
        public bool Gone { get; set; }
    }

    public class Listing
    {
        public DateTime Indexed { get; set; }
        public Account Account { get; set; }
        public Price Price { get; set; }
    }

    public class Price
    {
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }

    }

    public class Account
    {
        public string Name { get; set; }
        public string LastCharacterName { get; set; }
    }

    public class ItemListing
    {
        public string Name { get; set; }
        public string TypeLine { get; set; }
        public bool Verified { get; set; }
        public bool Identified { get; set; }
        public int Ilvl { get; set; }
    }
}
