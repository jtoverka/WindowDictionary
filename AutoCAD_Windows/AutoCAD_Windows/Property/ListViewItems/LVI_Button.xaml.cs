using System.Windows.Controls;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for ListItemButton.xaml
    /// </summary>
    public partial class LVI_Button : ListViewItem
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.TextBlock">TextBlock</see> element.
        /// </summary>
        public TextBlock TextBlock
        {
            get { return this.textblock; }
        }

        /// <summary>
        /// Gets the <see cref="System.Windows.Controls.Button">Button</see> element.
        /// </summary>
        public Button Button
        {
            get { return this.button; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public LVI_Button()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion
    }
}