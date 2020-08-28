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
    /// <summary>
    /// 
    /// </summary>
    public class ListViewPropertyConverter : IValueConverter
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
            if (value == null)
                return new ListView();

            var propertyGroup = value as PropertyGroup;
            var propertyItems = propertyGroup.PropertyItems;
            var list = new ListView();
            object root;

            foreach (PropertyItem item in propertyItems)
            {
                switch (item.ValueType)
                {
                    case PropertyType.Boolean:
                        list.Items.Add(new ListItemBoolean()
                        {
                            IsChecked = System.Convert.ToBoolean(item.Values[0]),
                            Text = item.PropertyName,
                        });
                        break;
                    case PropertyType.Button:
                        var button = new ListItemButton()
                        {
                            Label = item.PropertyName,
                            ButtonText = item.Values[0].ToString(),
                            ParentGroup = propertyGroup,
                        };
                        
                        root = Root(propertyGroup);
                        if ((root != null) && (root.GetType() == typeof(PropertyCreator)))
                        {
                            if (item.ValueIndex == 0)
                                button.Click += (root as PropertyCreator).AddGroup_Button_Click;
                            else if (item.ValueIndex == 1)
                                button.Click += (root as PropertyCreator).AddProperty_Button_Click;
                        }
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
                        foreach(object element in item.Values)
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
                        var listItemString = new ListItemString()
                        {
                            Item = item,
                        };

                        root = Root(propertyGroup);

                        if ((root != null) && (root.GetType() == typeof(PropertyCreator)))
                        {
                            listItemString.TextModified += (root as PropertyCreator).Rename_Text_Input;
                        }

                        list.Items.Add(listItemString);
                        break;
                }
            }

            return list.Items;
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

        private object Root(PropertyGroup group)
        {
            if ((group.Parent != null) 
                && (group.Parent.GetType() == typeof(PropertyGroup)))
                return Root(group.Parent as PropertyGroup);
            else
                return group.Parent;
        }
    }
}
