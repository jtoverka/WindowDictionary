using System.Windows.Controls;
using System.ComponentModel;
using WindowDictionary.Resources;
using System.Windows.Input;
using System;
using System.Windows;
using System.Windows.Threading;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListItemInteger.xaml
    /// </summary>
    public partial class ListItemInteger : ListViewItem, INotifyPropertyChanged
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
        public ListItemInteger()
        {
            DataContext = this;

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Popup_Timer_Tick;

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
        private void TextBox_Integer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Integer_PreviewKeyDown(sender, e);
        }

        private void TextBox_Integer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_String_PreviewTextInput(sender, e);

            if (e.Handled == true)
                return;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            if (!Item.ValueRange.IsValid(text))
            {
                e.Handled = true;

                popupText.Text = "Value must be between " + Item.ValueRange.Min.ToString() + " and " + Item.ValueRange.Max.ToString() + "!";

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
    }
}
