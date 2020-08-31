using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for ListItemInteger.xaml
    /// </summary>
    public partial class LVI_TextBox : ListViewItem
    {
        #region Fields

        DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBlock">TextBlock</see> that represents the label.
        /// </summary>
        public TextBlock TextBlock
        {
            get { return this.textblock; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBox">TextBox</see> that represents the input box.
        /// </summary>
        public TextBox TextBox
        {
            get { return this.textbox; }
        }

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
        public LVI_TextBox()
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
