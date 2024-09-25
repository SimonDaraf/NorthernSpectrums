using System.Globalization;
using System.Windows.Data;

namespace NorthernSpectrums.MVVM.Model.Converters
{
    /// <summary>
    /// <c>Class</c> Handles the scaling convertion.
    /// </summary>
    class PedalKnobScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if valid double
            if (value is double scale && scale != 0)
            {
                return scale * (87f / 733f); // The scale relation.
            }

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
