using System;
using System.Globalization;
using System.Windows.Data;

namespace Sidekick.Presentation.Wpf.Converters
{
    public class StringToNullableDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double?)
            {
                var doubleValue = (double?)value;
                if (doubleValue.HasValue)
                {
                    return doubleValue.Value;
                }
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value.ToString();
            if (double.TryParse(stringValue, out var doubleValue))
            {
                return doubleValue;
            }
            return null;
        }
    }
}
