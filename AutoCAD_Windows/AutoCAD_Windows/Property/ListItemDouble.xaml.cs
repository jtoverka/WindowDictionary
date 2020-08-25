using System.Windows.Controls;
using System.ComponentModel;
using WindowDictionary.Resources;
using System.Windows.Input;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListItemInteger.xaml
    /// </summary>
    public partial class ListItemDouble : ListViewItem, INotifyPropertyChanged
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

        public ListItemDouble()
        {
            DataContext = this;

            InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void TextBox_Double_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewKeyDown(sender, e);
        }
        private void TextBox_Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewTextInput(sender, e);

            if (e.Handled == true)
                return;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            if (!Item.ValueRange.IsValid(text))
            {
                e.Handled = true;
                box.ToolTip = new ToolTip()
                {
                    Content = "Value must be between " + Item.ValueRange.Min.ToString() + " and " + Item.ValueRange.Max.ToString() + "!",
                    Visibility = System.Windows.Visibility.Visible,
                    IsOpen = true,
                    StaysOpen = false,
                };
            }
        }
    }
}
