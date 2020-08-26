using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using WindowDictionary.Property;
using System.Windows;

namespace WindowDictionary.Converters
{
    public class ListViewPropertyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var propertyGroup = value as ObservableCollection<PropertyItem>;
            var list = new ListView();

            foreach(PropertyItem item in propertyGroup)
            {
                switch (item.ValueType)
                {
                    case PropertyType.Boolean:
                        list.Items.Add(new ListItemBoolean()
                        {
                            IsChecked = System.Convert.ToBoolean(item.Value),
                            Text = item.PropertyName,
                        });
                        break;
                    case PropertyType.Double:
                        list.Items.Add(new ListItemDouble()
                        {
                            Item = item,
                        });
                        break;
                    case PropertyType.Integer:
                        list.Items.Add(new ListItemInteger()
                        {
                            Item = item,
                        });
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
                        list.Items.Add(new ListItemString()
                        {
                            Item = item,
                        });
                        break;
                    case PropertyType.StringNoSpecial:
                        list.Items.Add(new ListItemString()
                        {
                            Item = item,
                        });
                        break;
                    default:
                        break;
                }
            }

            return list.Items;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
