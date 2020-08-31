using System;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for ListViewSelection.xaml
    /// </summary>
    public partial class LVI_ValueList : ListViewItem
    {
        #region Fields

        private DispatcherTimer timer = new DispatcherTimer();

        #endregion
        
        #region Properties

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBlock">TextBlock</see> element.
        /// </summary>
        public TextBlock TextBlock
        {
            get { return this.textblock; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.ListView">ListView</see> element.
        /// </summary>
        public ListView ListView
        {
            get { return this.listview; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBox">TextBox</see> element.
        /// </summary>
        public TextBox TextBox
        {
            get { return this.textbox; } 
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Primitives.Popup">Popup</see> element.
        /// </summary>
        public Popup Popup
        {
            get { return this.popup; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBlock">TextBlock</see> element.
        /// </summary>
        public TextBlock PopupTextBlock
        {
            get { return this.popuptextblock; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Button">Button</see> element. Default usage as an add content button.
        /// </summary>
        public Button AddButton
        {
            get { return this.addbutton; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Button">Button</see> element. Default usage as a remove content button.
        /// </summary>
        public Button RemoveButton
        {
            get { return this.removebutton; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Button">Button</see> element. Default usage as a move selected content up Button.
        /// </summary>
        public Button UpButton
        {
            get { return this.upbutton; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Button">Button</see> element. Default usage as a move selected content down Button.
        /// </summary>
        public Button DownButton
        {
            get { return this.downbutton; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Button">Button</see> element. Default usage as a move selected content to top Button.
        /// </summary>
        public Button TopButton
        {
            get { return this.topbutton; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Button">Button</see> element. Default usage as a move selected content to bottom Button.
        /// </summary>
        public Button BottomButton
        {
            get { return this.bottombutton; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public LVI_ValueList()
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
            popuptextblock.Text = message;
            popuptextblock.MaxWidth = textbox.ActualWidth;
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
