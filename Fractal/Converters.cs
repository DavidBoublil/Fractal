using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using Point = Converters.Models.Point;

namespace Converters
{
    public class RelocateCoordinate : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            double originValue = 0;
            if (values[0] is double)
                originValue = (double)values[0];

            double incrementFactor = 0;
            if (values[1] is double)
                incrementFactor = (double?)values[1] ?? 0;

            double scale = 1;
            if (values.Length >= 3 )
                scale = (double)values[2];

            return originValue * scale + incrementFactor;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class NullToHidden : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}