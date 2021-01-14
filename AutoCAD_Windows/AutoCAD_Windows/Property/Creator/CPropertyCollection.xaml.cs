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
using WindowDictionary.Property.Creator;

namespace WindowDictionary.Property.Creator
{
    /// <summary>
    /// Interaction logic for CGroupCollection.xaml
    /// </summary>
    public partial class CPropertyCollection : ListViewItem
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
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(CPropertyCollection));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public CPropertyCollection()
        {
            DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Add_Property_Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroup group = PropertyItem.Parent;
            string name = "Property";
            for (long i = 1; i < long.MaxValue; i++)
            {
                if (group.IsPropertyGroupUnique(name + i))
                {
                    group.PropertyGroups.Add(Initialize.Property(name + i));
                    break;
                }
            }
        }

        #endregion
    }
}
