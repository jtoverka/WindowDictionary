using System;
using System.Globalization;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;

namespace WindowDictionary.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class LayerImageConverter : IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsState = System.Convert.ToBoolean(value);
            string property = parameter.ToString();

            if (IsState)
            {
                return property switch
                {
                    "IsVisible" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerOn.bmp")),
                    "IsFrozen" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerFrozen.bmp")),
                    "IsLocked" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerLocked.bmp")),
                    "Plot" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerPlot.bmp")),
                    _ => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/X.bmp")),
                };
            }
            else
            {
                return property switch
                {
                    "IsVisible" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerOff.bmp")),
                    "IsFrozen" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerThawed.bmp")),
                    "IsLocked" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerUnlocked.bmp")),
                    "Plot" => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/LayerNoPlot.bmp")),
                    _ => new BitmapImage(new Uri("pack://application:,,,/WindowDictionary;component/Application/X.bmp")),
                };
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
