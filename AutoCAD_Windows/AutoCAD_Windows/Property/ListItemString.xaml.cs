using System.Windows.Controls;
using System.ComponentModel;
using WindowDictionary.Resources;
using System.Windows.Input;
using System;
using System.Windows.Threading;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListItemString.xaml
    /// </summary>
    public partial class ListItemString : ListViewItem, INotifyPropertyChanged
    {
        private DispatcherTimer timer = new DispatcherTimer();


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
        public ListItemString()
        {
            DataContext = this;

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Popup_Timer_Tick;

            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        public event TextChangedEventHandler TextModified;

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void TextBox_String_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            WindowDictionary.Resources.UILibrary.TextBox_String_PreviewTextInput(sender, e);

            if (e.Handled == true)
                return;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            var valid = true;
            
            if (Item.ValueRange != null)
            {
                for (int i = 0; (i < text.Length) && valid; i++)
                {
                    char character = text[i];
                    if (!Item.ValueRange.IsValid(character))
                        valid = false;
                }
            }

            if (!valid)
            {
                e.Handled = true;

                popupText.Text = "Characters allowed are between " + Item.ValueRange.Min.ToString() + " and " + Item.ValueRange.Max.ToString() + "!";

                popupText.MaxWidth = box.ActualWidth;
                popup.Width = box.ActualWidth;

                popup.IsOpen = false;
                popup.IsOpen = true;

                timer.Stop();
                timer.Start();
            }
        }

        private void Popup_Timer_Tick(object sender, EventArgs e)
        {
            popup.IsOpen = false;
            timer.Stop();
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            Item.Values[0] = box.Text;
            TextModified?.Invoke(this, e);
        }
    }
}
