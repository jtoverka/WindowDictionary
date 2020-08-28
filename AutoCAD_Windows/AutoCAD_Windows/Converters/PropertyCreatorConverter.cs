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
    public class PropertyCreatorConverter : IValueConverter
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

            var itemParent = value as PropertyGroup;
            var propertyItems = itemParent.PropertyItems;
            var list = new ListView();
            
            object root;

            foreach (PropertyItem item in propertyItems)
            {
                int itemValueIndex = item.ValueIndex;
                Range range = item.ValueRange;
                PropertyType itemValueType = item.ValueType;
                string itemPropertyName = item.PropertyName;

                root = Root(itemParent);

                switch (item.ValueType)
                {
                    case PropertyType.Boolean:
                        if (itemValueIndex == 4)
                        {
                            var button = new ListItemButton()
                            {
                                Label = itemPropertyName,
                                ButtonText = item.Values[0].ToString(),
                                ParentGroup = itemParent,
                            };

                            button.Click += (root as PropertyCreator).AddProperty_Button_Click;
                            list.Items.Add(button);
                        }
                        else
                        if (itemValueIndex == 3)
                        {
                            var button = new ListItemButton()
                            {
                                Label = itemPropertyName,
                                ButtonText = item.Values[0].ToString(),
                                ParentGroup = itemParent,
                            };

                            button.Click += (root as PropertyCreator).AddGroup_Button_Click;
                            list.Items.Add(button);
                        }
                        else
                        if (itemValueIndex == 2)
                        {
                            var button = new ListItemButton()
                            {
                                Label = itemPropertyName,
                                ButtonText = item.Values[0].ToString(),
                                ParentGroup = itemParent,
                            };

                            button.Click += (root as PropertyCreator).DeleteGroup_Button_Click;
                            list.Items.Add(button);
                        }
                        else 
                        if (itemValueIndex == 1)
                        {

                            var listViewSelection = new ListViewSelection()
                            {
                                Label = itemPropertyName,
                                Item = item,
                            };
                            listViewSelection.TextBox_PreviewKeyDown += (root as PropertyCreator).text_PreviewKeyDown;
                            listViewSelection.TextBox_PreviewTextInput += (root as PropertyCreator).text_PreviewTextInput;
                            listViewSelection.CollectionChanged += (root as PropertyCreator).ListItems_CollectionChanged;
                            listViewSelection.AddClick += (root as PropertyCreator).Add_Value_Button_Click;
                            list.Items.Add(listViewSelection);
                        }
                        else 
                        if (itemValueIndex == 0)
                        {
                            list.Items.Add(new ListItemBoolean()
                            {
                                IsChecked = System.Convert.ToBoolean(item.Values[0]),
                                Text = itemPropertyName,
                            });
                        }
                        
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
