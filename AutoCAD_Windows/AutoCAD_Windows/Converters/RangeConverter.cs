using System;
using System.Globalization;
using System.Windows.Data;
using WindowDictionary.Property.Logic;

namespace WindowDictionary.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class RangeConverter : IValueConverter
    {
        /// <summary>
        /// Convert a ValueRange property to a combobox index
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() == typeof(CharRange))
                return 0;
            if (value.GetType() == typeof(DoubleRange))
                return 1;
            if (value.GetType() == typeof(IntegerRange))
                return 2;

            return 0;
        }
        /// <summary>
        /// Converte a combobox index to a value range
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = System.Convert.ToInt32(value);

            if ((index == 0) || (index == -1))
                return new CharRange();
            if (index == 1)
                return new DoubleRange();
            if (index == 2)
                return new IntegerRange();

            return null;
        }
    }
}
