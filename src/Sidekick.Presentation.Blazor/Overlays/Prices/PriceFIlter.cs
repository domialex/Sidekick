using System.ComponentModel;
using Sidekick.Domain.Game.Modifiers.Models;
using Sidekick.Domain.Game.Trade.Models;

namespace Sidekick.Presentation.Blazor.Overlays.Prices
{
    public class PriceFilter : INotifyPropertyChanged
    {
#pragma warning disable 67
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore 67

        public PropertyFilterType? PropertyType { get; set; }

        public bool Enabled { get; set; }

        public string Id { get; set; }

        public string Text { get; set; }

        public bool HasRange { get; set; }

        public double? Min { get; set; }

        public double? Max { get; set; }

        public ModifierOption Option { get; set; }
    }
}
