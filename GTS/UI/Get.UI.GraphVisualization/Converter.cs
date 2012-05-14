using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace Get.UI
{
    public static class Converters
    {
        public static PointAdderConverter PointAdderConverter = new PointAdderConverter();
        public static Point Add(this Point p,int x,int y)
        {
            return new Point(p.X+x,p.Y+y);
        }
    }
    public sealed class PointAdderConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (!value.GetType().Equals((targetType))) return value;
            if (parameter == null) return value;
            Point pointValue = (Point)value;
            Point pointParameter = (Point)parameter;
            pointValue.X = pointValue.X + pointParameter.X;
            pointValue.Y = pointValue.Y + pointParameter.Y;

            return pointValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
