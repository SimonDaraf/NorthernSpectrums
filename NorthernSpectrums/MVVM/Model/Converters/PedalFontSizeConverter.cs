using System.Globalization;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    class PedalFontSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double height && height != 0)
            {
                double size = height * (15f / 30f);
                return size;
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
