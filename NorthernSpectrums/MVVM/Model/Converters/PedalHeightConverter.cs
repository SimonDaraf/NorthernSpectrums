using System.Globalization;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    /// <summary>
    /// <c>Class</c> Makes sure that the pedal container height preserves the correct aspect ratio based on width.
    /// </summary>
    class PedalHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double width && width != 0)
            {
                return width * (438f / 733f);
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
