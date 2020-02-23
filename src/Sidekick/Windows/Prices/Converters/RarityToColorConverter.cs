using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Sidekick.Business.Parsers.Models;

namespace Sidekick.Windows.Prices.Converters
{
    public class RarityToColorConverter : IValueConverter
    {
        private Color GetRarityColor(Item item)
        {
            return item?.Rarity switch
            {
                Rarity.Normal => Color.FromRgb(200, 200, 200),
                Rarity.Magic => Color.FromRgb(136, 136, 255),
                Rarity.Rare => Color.FromRgb(255, 255, 119),
                Rarity.Unique => Color.FromRgb(175, 96, 37),
                _ => Color.FromRgb(170, 158, 130),
            };
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(GetRarityColor((Item)value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
