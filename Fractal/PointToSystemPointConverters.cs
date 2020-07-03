using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Fractal.Models;

namespace Fractal
{
    public class PointToSystemPointConverters : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var shape = values[0] as Point;
            var origin = new Point((double)values[1], (double)values[2]);

            PointCollection pointCollection = new PointCollection();

            for (Point p = shape; p != null; p++)
            {
                var temp = p + origin;
                pointCollection.Add(new System.Windows.Point(temp.X, temp.Y));
            }

            return pointCollection;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}