using System.Windows.Controls;
using System.ComponentModel;
using WindowDictionary.Resources;
using System.Windows.Input;
using System;
using System.Windows;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListItemSelection.xaml
    /// </summary>
    public partial class ListItemSelection : ListViewItem, INotifyPropertyChanged
    {
        
        private PropertyItem _Item;
        /// <summary>
        /// 
        /// </summary>
        public PropertyItem Item
        {
            get { return _Item; }
            set
            {
                if (_Item == value)
                    return;
                                
                _Item = value;
                OnPropertyChanged("Item");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public ListItemSelection()
        {
            DataContext = this;

            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
