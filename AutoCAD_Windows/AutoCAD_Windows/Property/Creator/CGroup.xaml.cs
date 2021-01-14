using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WindowDictionary.Property.Creator
{
    /// <summary>
    /// Interaction logic for CGroup.xaml
    /// </summary>
    public partial class CGroup : ListViewItem
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the PropertyItem object.
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
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(CGroup));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public CGroup()
        {
            DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Delete_Group_Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroup PropertyItemParent = PropertyItem.Parent;
            IPropertyControl parent = PropertyItemParent.Parent;
            parent.PropertyGroups.Remove(PropertyItemParent);
        }

        /// <summary>
        /// Update the title property from PropertyGroup object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((!(sender is TextBox box))
                || (!this.IsLoaded))
                return;

            if (box.Text == PropertyItem.Name)
                return;

            if (this.PropertyItem.Parent.Parent.IsPropertyGroupUnique(box.Text))
            {
                box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Group names must be unique", "Warning", MessageBoxButton.OK);
                box.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            }
        }

        #endregion
    }
}
