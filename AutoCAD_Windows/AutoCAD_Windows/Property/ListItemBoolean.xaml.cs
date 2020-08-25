using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListItemBoolean.xaml
    /// </summary>
    public partial class ListItemBoolean : ListViewItem, INotifyPropertyChanged
    {

        private bool _IsChecked;
        /// <summary>
        /// 
        /// </summary>
        public bool IsChecked
        {
            get { return this._IsChecked; }
            set
            {
                if (this._IsChecked == value)
                    return;

                this._IsChecked = value;

                if (value)
                    OnChecked();
                else
                    OnUnChecked();

                OnPropertyChanged("IsChecked");
            }
        }


        private string _Text;
        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get { return this._Text; }
            set
            {
                if (this._Text == value)
                    return;

                this._Text = value;
                OnPropertyChanged("Text");
            }
        }

        public ListItemBoolean()
        {
            InitializeComponent();
        }
        public event RoutedEventHandler Checked;
        public event RoutedEventHandler UnChecked;
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void OnChecked()
        {
            Checked?.Invoke(this, new RoutedEventArgs());
        }
        private void OnUnChecked()
        {
            UnChecked?.Invoke(this, new RoutedEventArgs());
        }
    }
}
