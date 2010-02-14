using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;
using System.Globalization;

[assembly: XmlnsDefinition("http://schemas.get.com/winfx/2009/xaml/Converter", "Get.Common.Converter")]
namespace Get.Common.Converter
{
    public static class Converters
    {
        public static StringToStringConverter StringToStringConverter = new StringToStringConverter();
        public static StringsToStringConverter StringsToStringConverter = new StringsToStringConverter();
        public static StringCutterConverter StringCutterConverter = new StringCutterConverter();
        public static BooleanToVisibilityConverter BooleanToVisibilityConverter = new BooleanToVisibilityConverter();
    }

    public sealed class StringToStringConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (!value.GetType().Equals(typeof(string))) return string.Empty;
            if (parameter == null) return value;

            return value.ToString() + string.Empty + parameter;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (!value.GetType().Equals(typeof(string))) return string.Empty;
            if (parameter == null) return value;

            if (!value.ToString().EndsWith(parameter.ToString())) return value;

            if (value.ToString().Length < 0) return value;

            return value.ToString().Remove(value.ToString().Length-1,1);

        }

        #endregion
    }
    public sealed class StringsToStringConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Member

        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return "asdf";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public sealed class StringCutterConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null) return string.Empty;
            if (!value.GetType().Equals(typeof(string))) return string.Empty;
            if((value.Equals(string.Empty))) return value;
            int number;

            if (!Int32.TryParse(parameter.ToString(), out number)) return value.ToString();

            string StringCv = value.ToString();
            if (StringCv.Length < number || StringCv.Length.Equals(number)) return value.ToString();

            StringCv = StringCv.Remove(0, StringCv.Length - number);


            return "..." + StringCv;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();

        }

        #endregion
    }

    /// <summary>
    /// Stellt den Konverter dar, der boolesche Werte in und von Visibility-Enumerationswerten konvertiert. 
    /// http://msdn.microsoft.com/de-de/library/system.windows.controls.booleantovisibilityconverter.aspx
    /// </summary>
    [Localizability(LocalizationCategory.NeverLocalize)]
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool flag = false;
            if (value is bool)
            {
                flag = (bool)value;
            }
            else if (value is bool?)
            {
                bool? nullable = (bool?)value;
                flag = nullable.HasValue ? nullable.Value : false;
            }
            return (flag ? Visibility.Visible : Visibility.Collapsed);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((value is Visibility) && (((Visibility)value) == Visibility.Visible));
        }
        #endregion
    }


}
