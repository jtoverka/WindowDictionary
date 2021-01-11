using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Threading;
using WindowDictionary.Resources;

namespace WindowDictionary.Property.Editor
{
    /// <summary>
    /// Interaction logic for ListItemInteger.xaml
    /// </summary>
    public partial class ETextBox : ListViewItem
    {
        #region Fields

        readonly DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the <see cref="PropertyItem"/> element/>
        /// </summary>
        public PropertyItem PropertyItem
        {
            get { return (PropertyItem)GetValue(PropertyItemProperty); }
            set { SetValue(PropertyItemProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for PropertyItem.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PropertyItemProperty =
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(ETextBox));

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Primitives.Popup">Popup</see> that represents the popup.
        /// </summary>
        public Popup Popup
        {
            get { return this.popup; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBlock">TextBlock</see> that represents the popup text.
        /// </summary>
        public TextBlock PopupTextBlock
        {
            get { return this.popuptext; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ETextBox()
        {
            DataContext = this;

            timer.Interval = TimeSpan.FromSeconds(4);
            timer.Tick += Popup_Timer_Tick;

            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Displays popup with error message 
        /// </summary>
        /// <param name="message"></param>
        public void TriggerPopup(string message)
        {
            popuptext.Text = message;
            popuptext.MaxWidth = textbox.ActualWidth;
            popup.Width = textbox.ActualWidth;
            popup.IsOpen = false;
            popup.IsOpen = true;

            timer.Stop();
            timer.Start();
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

            TextBox textbox = sender as TextBox;
            ListViewItem listviewitem = textbox.DataContext as ListViewItem;
            PropertyItem Item = listviewitem.Tag as PropertyItem;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            Regex expression = new Regex(Item.Regex);
            if (!expression.IsMatch(text))
            {
                e.Handled = true;

            }
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

            TextBox textbox = sender as TextBox;
            ListViewItem listviewitem = textbox.DataContext as ListViewItem;
            PropertyItem Item = listviewitem.Tag as PropertyItem;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            Regex expression = new Regex(Item.Regex);
            if (!expression.IsMatch(text))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Popup_Timer_Tick(object sender, EventArgs e)
        {
            popup.IsOpen = false;
            timer.Stop();
        }

        #endregion
    }
}
