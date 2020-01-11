using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Sidekick.Windows.Overlay.Converters
{
    public class RarityToColorConverter : IValueConverter
    {
        private Color GetRarityColor(string rarity)
        {
            switch (rarity)
            {
                case "Normal":
                    return Color.FromRgb(200, 200, 200);
                case "Magic":
                    return Color.FromRgb(136, 136, 255);
                case "Rare":
                    return Color.FromRgb(255, 255, 119);
                case "Unique":
                    return Color.FromRgb(175, 96, 37);
                default:
                    //gem, currency, divination card, quest item, prophecy, relic
                    return Color.FromRgb(170, 158, 130);
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string rarity = value.ToString();
            Color color = GetRarityColor(rarity);

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
