using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Fractal.Models;

namespace Fractal
{
    public class Converters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            PointCollection pointCollection = new PointCollection();

            for (Point p = value as Point; p != null; p++)
            {
                pointCollection.Add(new System.Windows.Point(p.X, p.Y));
            }

            return pointCollection;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}