using System;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Graph", "Get.UI")]
namespace DataStructures.UI
{
    public static class Converters
    {
        public static PointAdderConverter PointAdderConverter = new PointAdderConverter();
        public static DoubleAdderConverter DoubleAdderConverter = new DoubleAdderConverter();

        public static Point Add(this Point p, int x, int y)
        {
            return new Point(p.X + x, p.Y + y);
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

            Point pointValue= (Point)value;
            Point pointParameter = new Point();

            //if (parameter.GetType().Equals(typeof(String)))
            //{
            //    pointParameter = (Point)parameter; 
            //}else
             if (parameter.GetType().Equals(typeof(Point)))
            {
                pointParameter = (Point)parameter;
            }

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
    public sealed class DoubleAdderConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (!value.GetType().Equals((targetType))) return value;
            if (parameter == null) return value;

            Double DoubleValue = (Double)value;
            Double DoubleParameter=0;

            //if (parameter.GetType().Equals(typeof(String)))
            //{
            //    pointParameter = (Point)parameter; 
            //}else
            if (parameter.GetType().Equals(typeof(Double)))
            {
                DoubleParameter = (Double)parameter;
            }
            else if (parameter.GetType().Equals(typeof(String)))
            {
                Double.TryParse(parameter.ToString(), out DoubleParameter);
            }

            DoubleValue = DoubleValue + DoubleParameter;


            return DoubleValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value;
        }

        #endregion
    }
}
