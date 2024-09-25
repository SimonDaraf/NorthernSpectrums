using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    // Used for a responsive layout.
    public class SliderHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                return width * (175d / 1840d);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandOneMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (384d / 1840d);

                return new Thickness(0, 0, xMargin, yMargin);
            }
            return new Thickness(0,0,0,0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandTwoMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (169d / 1840d);

                return new Thickness(0, 0, xMargin, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandThreeMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (46d / 1840d);

                return new Thickness(xMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandFourMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (261d / 1840d);

                return new Thickness(xMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandFiveMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (476d / 1840d);

                return new Thickness(xMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandSixMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (691d / 1840d);

                return new Thickness(xMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandSevenMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (906d / 1840d);

                return new Thickness(xMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BandEightMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double yMargin = width * (21d / 1840d);
                double xMargin = width * (1122d / 1840d);

                return new Thickness(xMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
