using System;
using System.Globalization;
using System.Windows.Data;
using System.Collections.Generic;
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
            if (value == null)
                return new ListView();

            var propertyGroup = value as PropertyGroup;
            var propertyItems = propertyGroup.PropertyItems;
            var list = new ListView();

            foreach(PropertyItem item in propertyItems)
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
                    case PropertyType.Button:
                        var button = new ListItemButton()
                        {
                            Label = item.PropertyName,
                            ButtonText = item.Value.ToString(),
                            ParentGroup = propertyGroup,
                        };

                        button.Click += item.EventHandler;

                        list.Items.Add(button);
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
                        ObservableCollection<string> collection = new ObservableCollection<string>();
                        foreach(object element in item.ValueSelection as ObservableCollection<object>)
                        {
                            collection.Add(element.ToString());
                        }

                        var listItem = new ListItemSelection()
                        {
                            Item = item,
                        };

                        list.Items.Add(listItem);
                        break;
                    case PropertyType.SelectionEditDouble:
                        break;
                    case PropertyType.SelectionEditInteger:
                        break;
                    case PropertyType.SelectionEditString:
                        break;
                    case PropertyType.String:
                        list.Items.Add(new ListItemString()
                        {
                            Item = item,
                        });
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
