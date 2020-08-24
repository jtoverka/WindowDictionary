using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Controls;
using WindowDictionary.Property;

namespace WindowDictionary.Converters
{
    public class ListViewPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var propertyGroup = value as PropertyGroup;
            foreach(PropertyItem item in propertyGroup.PropertyItems)
            {

                switch (item.ValueType)
                {
                    case PropertyType.Boolean:
                        return new ListItemBoolean()
                        {
                            IsChecked = System.Convert.ToBoolean(item.Value),
                            Text = item.PropertyName,
                        };
                    case PropertyType.Double:
                        return new ListItemBoolean()
                        {
                            IsChecked = System.Convert.ToBoolean(item.Value),
                            Text = item.PropertyName,
                        };
                    case PropertyType.Integer:
                        break;
                    case PropertyType.SelectionString:
                        break;
                    case PropertyType.SelectionEditDouble:
                        break;
                    case PropertyType.SelectionEditInteger:
                        break;
                    case PropertyType.SelectionEditStringAll:
                        break;
                    case PropertyType.SelectionEditStringNoSpecial:
                        break;
                    case PropertyType.StringAll:
                        break;
                    case PropertyType.StringNoSpecial:
                        break;
                    default:
                        break;
                }
            }
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
