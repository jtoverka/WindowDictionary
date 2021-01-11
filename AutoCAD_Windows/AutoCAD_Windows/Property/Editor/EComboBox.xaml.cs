using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WindowDictionary.Property.Editor
{
    /// <summary>
    /// Interaction logic for ListItemSelection.xaml
    /// </summary>
    public partial class EComboBox : ListViewItem
    {
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
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(EComboBox));

        /// <summary>
        /// Gets the <see cref="Popup">Popup</see> element.
        /// </summary>
        public Popup Popup
        {
            get { return this.popup; }
        }

        /// <summary>
        /// Gets the <see cref="TextBlock">TextBlock</see> element within the <see cref="Popup">Popup</see> element.
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
        public EComboBox()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion
    }
}