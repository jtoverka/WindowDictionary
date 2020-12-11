using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for ListItemSelection.xaml
    /// </summary>
    public partial class LVI_ComboBox : ListViewItem
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="TextBlock">TextBlock</see> element.
        /// </summary>
        public TextBlock TextBlock
        {
            get { return this.textblock; }
        }

        /// <summary>
        /// Gets the <see cref="ComboBox">ComboBox</see> element.
        /// </summary>
        public ComboBox ComboBox
        {
            get { return this.combobox; }
        }

        /// <summary>
        /// Gets the <see cref="Primitives.Popup">Popup</see> element.
        /// </summary>
        public Popup Popup
        {
            get { return this.popup; }
        }

        /// <summary>
        /// Gets the <see cref="TextBlock">TextBlock</see> element within the <see cref="Primitives.Popup">Popup</see> element.
        /// </summary>
        public TextBlock PopupTextBlock
        {
            get { return this.popupText; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public LVI_ComboBox()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion
    }
}