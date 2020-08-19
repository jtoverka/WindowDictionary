using System;
using System.Globalization;
using System.Windows.Data;
using System.IO;
using System.Windows.Media.Imaging;

namespace AutoCAD_Windows.Converters
{
    public class LayerImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool IsState = System.Convert.ToBoolean(value);
            string property = parameter.ToString();

            if (IsState)
            {
                return property switch
                {
                    "IsVisible" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerOn.bmp")),
                    "IsFrozen" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerFrozen.bmp")),
                    "IsLocked" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerLocked.bmp")),
                    "Plot" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerPlot.bmp")),
                    _ => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/X.bmp")),
                };
            }
            else
            {
                return property switch
                {
                    "IsVisible" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerOff.bmp")),
                    "IsFrozen" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerThawed.bmp")),
                    "IsLocked" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerUnlocked.bmp")),
                    "Plot" => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/LayerNoPlot.bmp")),
                    _ => new BitmapImage(new Uri("pack://application:,,,/AutoCAD_Windows;component/Application/X.bmp")),
                };
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
