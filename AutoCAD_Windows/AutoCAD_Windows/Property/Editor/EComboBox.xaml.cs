using System.Text.RegularExpressions;
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
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(EComboBox));

        #endregion
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

        #region Delegates, Events, Handlers

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(sender is ComboBox box))
                return;

            Regex regex = new Regex(this.PropertyItem.Regex);
            if (!regex.IsMatch(box.SelectedItem.ToString()))
            {
                MessageBox.Show(this.PropertyItem.Help, "Error", MessageBoxButton.OK);

                if (e.RemovedItems.Count > 0)
                    box.SelectedItem = e.RemovedItems[0];
            }
        }

        private void Property_Loaded(object sender, RoutedEventArgs e)
        {
            if (PropertyItem.Values.Count > 0)
                PropertyItem.SelectedValue = PropertyItem.Values[0];
        }

        #endregion
    }
}