using System;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Data;

namespace WindowDictionary.Converters
{
    public class LayerTransparencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.Convert.ToString(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                return System.Convert.ToSByte(value);
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
