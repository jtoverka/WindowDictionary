using System.Windows.Controls;
using System.ComponentModel;
using WindowDictionary.Resources;
using System.Windows.Input;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListItemInteger.xaml
    /// </summary>
    public partial class ListItemInteger : ListViewItem, INotifyPropertyChanged
    {
        private PropertyItem _Item;
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

        public ListItemInteger()
        {
            DataContext = this;

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        private void TextBox_Integer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Integer_PreviewKeyDown(sender, e);
        }
        private void TextBox_Integer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_Integer_PreviewTextInput(sender, e);
        }
    }
}
