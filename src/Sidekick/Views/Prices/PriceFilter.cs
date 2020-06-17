using System.ComponentModel;
using Sidekick.Business.Apis.Poe.Models;

namespace Sidekick.Views.Prices
{
    public class PriceFilter : INotifyPropertyChanged
    {
        public string Type { get; set; }

        public bool Enabled { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        public bool HasRange { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public bool ApplyNegative { get; set; }

        public ModifierOption Option { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
