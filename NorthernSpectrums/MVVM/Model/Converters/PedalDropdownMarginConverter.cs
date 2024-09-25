using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    class PedalDropdownLowerMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height != 0)
            {
                double yMargin = height * (26f / 733f);
                return new Thickness(0, 0, 0, yMargin);
            }

            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PedalDropdownUpperMarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height != 0)
            {
                double yMargin = height * (255f / 733f);
                return new Thickness(0, 0, 0, yMargin);
            }

            return new Thickness(0, 0, 0, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
