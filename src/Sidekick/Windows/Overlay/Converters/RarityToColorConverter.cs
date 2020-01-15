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
            if (rarity == Legacy.LanguageProvider.Language.RarityNormal)
            {
                return Color.FromRgb(200, 200, 200);
            }
            if (rarity == Legacy.LanguageProvider.Language.RarityMagic)
            {
                return Color.FromRgb(136, 136, 255);
            }
            if (rarity == Legacy.LanguageProvider.Language.RarityRare)
            {
                return Color.FromRgb(255, 255, 119);
            }
            if (rarity == Legacy.LanguageProvider.Language.RarityUnique)
            {
                return Color.FromRgb(175, 96, 37);
            }

            // Gem, Currency, Divination Card, Quest Item, Prophecy, Relic, etc.
            return Color.FromRgb(170, 158, 130);
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
