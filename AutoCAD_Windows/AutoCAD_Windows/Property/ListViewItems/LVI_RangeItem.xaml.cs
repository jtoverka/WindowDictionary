using System;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for LVI_RangeItem.xaml
    /// </summary>
    public partial class LVI_RangeItem : ListViewItem
    {
        #region Fields

        readonly DispatcherTimer maxTimer = new DispatcherTimer();
        readonly DispatcherTimer minTimer = new DispatcherTimer();

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Logic Gate CheckBox
        /// </summary>
        public CheckBox CheckBox
        {
            get { return this.checkbox; }
        }

        /// <summary>
        /// Gets the Range type ComboBox
        /// </summary>
        public ComboBox ComboBox
        {
            get { return this.combobox; }
        }

        /// <summary>
        /// Gets the Min TextBox
        /// </summary>
        public TextBox Min
        {
            get { return this.min; }
        }

        /// <summary>
        /// Gets the Max TextBox
        /// </summary>
        public TextBox Max
        {
            get { return this.max; }
        }

        /// <summary>
        /// Gets the new range button.
        /// </summary>
        public Button Button
        {
            get { return this.button; }
        }

        /// <summary>
        /// Gets the delete range button.
        /// </summary>
        public Button Delete
        {
            get { return this.delete; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public LVI_RangeItem()
        {
            DataContext = this;

            maxTimer.Interval = TimeSpan.FromSeconds(4);
            maxTimer.Tick += MaxTimer_Tick;
            minTimer.Interval = TimeSpan.FromSeconds(4);
            minTimer.Tick += MinTimer_Tick;

            InitializeComponent();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Trigger popup error on Minimum TextBox
        /// </summary>
        /// <param name="error"></param>
        public void TriggerMinPopup(string error)
        {
            minTimer.Stop();
            minTimer.Start();
            min_popup.IsOpen = false;
            min_popupText.Text = error;
            min_popup.IsOpen = true;
        }

        /// <summary>
        /// Trigger popup error on Maximum TextBox
        /// </summary>
        /// <param name="error"></param>
        public void TriggerMaxPopup(string error)
        {
            maxTimer.Stop();
            maxTimer.Start();
            max_popup.IsOpen = false;
            max_popupText.Text = error;
            max_popup.IsOpen = true;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Stop timer for minimum TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinTimer_Tick(object sender, EventArgs e)
        {
            minTimer.Stop();
            min_popup.IsOpen = false;
        }

        /// <summary>
        /// Stop timer for maximum TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxTimer_Tick(object sender, EventArgs e)
        {
            maxTimer.Stop();
            max_popup.IsOpen = false;
        }

        #endregion
    }
}
