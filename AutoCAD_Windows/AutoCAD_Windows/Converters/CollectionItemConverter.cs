using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Windows.Data;

namespace WindowDictionary.Converters
{
    class CollectionItemConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as ObservableCollection<object>;
            var index = System.Convert.ToInt32(parameter);
            
            return collection[index];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as ObservableCollection<object>;
            var index = System.Convert.ToInt32(parameter);

            return collection[index];
        }
    }
}
