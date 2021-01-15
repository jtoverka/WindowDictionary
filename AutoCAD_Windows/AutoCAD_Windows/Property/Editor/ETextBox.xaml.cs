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
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(ETextBox));
        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public ETextBox()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region Delegates, Events, Handlers

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox box)
                || (!this.IsLoaded))
                return;

            Regex regex = new Regex(this.PropertyItem.Regex);

            if (regex.IsMatch(box.Text))
            {
                box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
            else
            {
                e.Handled = true;
                MessageBox.Show(this.PropertyItem.Help, "Error", MessageBoxButton.OK);
                box.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
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
