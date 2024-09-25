using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    // Handles responsive layout for racks.
    public class KnobSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                return width * (79f / 1840f);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KnobLConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (57f / 1840f);
                double xMargin = width * (106f / 1840f);

                return new Thickness(xMargin, yMargin, 0, 0);
            }
            return new Thickness(0,0,0,0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KnobMConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (57f / 1840f);
                double xMargin = width * (620f / 1840f);

                return new Thickness(xMargin, yMargin, 0, 0);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class KnobRConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (57f / 1840f);
                double xMargin = width * (1133f / 1840f);

                return new Thickness(xMargin, yMargin, 0, 0);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
