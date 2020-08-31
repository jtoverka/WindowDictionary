using System.Windows.Controls;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for ListItemBoolean.xaml
    /// </summary>
    public partial class LVI_CheckBox : ListViewItem
    {
        #region Property

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.CheckBox">CheckBox</see> element.
        /// </summary>
        public CheckBox CheckBox
        {
            get { return this.checkbox; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBlock">TextBlock</see> element.
        /// </summary>
        public TextBlock TextBlock
        {
            get { return this.textblock; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public LVI_CheckBox()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion
    }
}
