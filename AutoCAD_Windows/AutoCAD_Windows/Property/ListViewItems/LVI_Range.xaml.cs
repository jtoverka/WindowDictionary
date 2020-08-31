using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for LVI_Range.xaml
    /// </summary>
    public partial class LVI_Range : ListViewItem, INotifyPropertyChanged
    {

        private bool _IsCHecked;
        /// <summary>
        /// 
        /// </summary>
        public bool IsCHecked
        {
            get { return _IsCHecked; }
            set
            {
                if (_IsCHecked == value)
                    return;

                _IsCHecked = value;
                OnPropertyChanged("IsCHecked");
            }
        }

        private Logic.LogicalOperator _SelectedOperator;
        /// <summary>
        /// 
        /// </summary>
        public Logic.LogicalOperator SelectedOperator
        {
            get { return _SelectedOperator; }
            set
            {
                if (_SelectedOperator == value)
                    return;

                _SelectedOperator = value;
                OnPropertyChanged("SelectedOperator");
            }
        }
        /// <summary>
        /// Represents a range object
        /// </summary>
        public LVI_Range()
        {
            DataContext = this;

            combobox.SetBinding(ComboBox.ItemsSourceProperty, new Binding()
            {
                Source = Enum.GetValues(typeof(Logic.LogicalOperator)),
            });

            InitializeComponent();
        }

        /// <summary>
        /// Invoked when a component property is modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the property changed event.
        /// </summary>
        /// <param name="property"></param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
