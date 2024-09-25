using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    /// <summary>
    /// <c>Class</c> Converts the pedal height to supply a valid margin.
    /// </summary>
    class PedalKnobMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height != 0)
            {
                double yMargin = height * (-397f / 733f);
                return new Thickness(0, yMargin, 0, 0);
            }

            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PedalKnobLeftUpMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is double width && width != 0)
            {
                double xMargin = width * (258f / 438f);
                double yMargin = width * (-397f / 438f);
                return new Thickness(0, yMargin, xMargin, 0);
            }

            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PedalKnobRightUpMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value is double width && width != 0)
            {
                double xMargin = width * (-258f / 438f);
                double yMargin = width * (-397f / 438f);
                return new Thickness(0, yMargin, xMargin, 0);
            }

            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
