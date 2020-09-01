using System;
using System.Globalization;
using System.Windows.Data;
using WindowDictionary.Property.Logic;

namespace WindowDictionary.Converters
{
    /// <summary>
    /// Convert a LogicGate to and from a combobox index
    /// </summary>
    public class LogicRangeConverter : IValueConverter
    {
        /// <summary>
        /// Convert a logic gate to a combobox index
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is LogicalGate gate))
                gate = new LogicalGate();

            return (int)gate.Operator;
        }

        /// <summary>
        /// Convert a combobox index to a logic gate
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int index = System.Convert.ToInt32(value);

            return new LogicalGate((LogicalOperator)index);
        }
    }
}
