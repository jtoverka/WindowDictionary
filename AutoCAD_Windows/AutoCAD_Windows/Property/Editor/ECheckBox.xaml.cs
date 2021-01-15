using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace WindowDictionary.Property.Editor
{
    /// <summary>
    /// Interaction logic for ListItemBoolean.xaml
    /// </summary>
    public partial class ECheckBox : ListViewItem
    {
        #region Property
        #region Property - PropertyItem : PropertyItem

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
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(ECheckBox));

        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ECheckBox()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Property_Loaded(object sender, RoutedEventArgs e)
        {
            if (PropertyItem.Values.Count > 0)
                PropertyItem.SelectedValue = PropertyItem.Values[0];
        }

        #endregion
    }
}
