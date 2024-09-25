using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    // These are just converters responsible for keeping the UI responsive.
    public class AmpKnobSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double scale && scale != 0)
            {
                return scale * (55f / 786f);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AmpLLKnobMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double scale && scale != 0)
            {
                double rMargin = scale * (714f / 1837f);
                double yMargin = scale * (201f / 1837f);
                return new Thickness(0, 0, rMargin, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AmpMLKnobMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double scale && scale != 0)
            {
                double rMargin = scale * (359f / 1837f);
                double yMargin = scale * (201f / 1837f);
                return new Thickness(0, 0, rMargin, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AmpMKnobMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double scale && scale != 0)
            {
                double rMargin = scale * (4f / 1837f);
                double yMargin = scale * (201f / 1837f);
                return new Thickness(0, 0, rMargin, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AmpRMKnobMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double scale && scale != 0)
            {
                double lMargin = scale * (350f / 1837f);
                double yMargin = scale * (201f / 1837f);
                return new Thickness(lMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class AmpRRKnobMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double scale && scale != 0)
            {
                double lMargin = scale * (705f / 1837f);
                double yMargin = scale * (201f / 1837f);
                return new Thickness(lMargin, 0, 0, yMargin);
            }
            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
