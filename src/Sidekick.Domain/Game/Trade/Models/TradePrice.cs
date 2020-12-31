using System;

namespace Sidekick.Domain.Game.Trade.Models
{
    public class TradePrice
    {
        public DateTimeOffset Date { get; set; }

        public string AccountName { get; set; }

        public string AccountCharacter { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; }

        public string Whisper { get; set; }
    }
}
