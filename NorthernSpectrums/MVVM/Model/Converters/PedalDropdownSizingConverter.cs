using System.Globalization;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    class PedalDropdownWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height != 0)
            {
                double width = height * (140f / 733f);
                return width;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    class PedalDropdownHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                double height = width * (30f / 438f);
                return height;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
