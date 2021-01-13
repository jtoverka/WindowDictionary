using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using WindowDictionary.Resources;

namespace WindowDictionary.Property.Creator
{
    /// <summary>
    /// Interaction logic for CPropertyDependency.xaml
    /// </summary>
    public partial class CPropertyDependency : Window
    {
        #region Properties
        #region Property - PropertyItem : PropertyItem

        /// <summary>
        /// Gets or Sets the parent PropertyItem object
        /// </summary>
        public PropertyItem PropertyItem
        {
            get { return (PropertyItem)GetValue(PropertyItemProperty); }
            internal set { SetValue(PropertyItemProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for PropertyItem.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PropertyItemProperty =
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(CPropertyDependency));

        #endregion
        #region Property - Result : DialogResult

        /// <summary>
        /// Gets the DialogResult of this window
        /// </summary>
        public DialogResult Result
        {
            get { return (DialogResult)GetValue(DialogResultProperty); }
            internal set { SetValue(DialogResultProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for DialogResult.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.Register("Result", typeof(DialogResult), typeof(CPropertyDependency));

        #endregion
        #region Property - DependencyItems : ObservableCollection<DependencyItem>

        /// <summary>
        /// Gets the collection of dependency items
        /// </summary>
        public ObservableCollection<DependencyItem> DependencyItems { get; } = new ObservableCollection<DependencyItem>();

        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public CPropertyDependency()
        {
            this.PropertyItem = new PropertyItem();
            Result = WindowDictionary.Resources.DialogResult.None;
            DataContext = this;
            InitializeComponent();
            rb1.IsChecked = !this.PropertyItem.Collapsible;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clears the dependencies list and updates the properties list
        /// </summary>
        public void UpdateList()
        {
            MainList.Items.Clear();
            DependencyList.Items.Clear();
            foreach (PropertyGroup item in this.PropertyItem.Parent.GetProperties())
            {
                MainList.Items.Add(item);
            }
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Add_Property_Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroup[] groups = MainList.SelectedItems.Cast<PropertyGroup>().ToArray();
            foreach (PropertyGroup item in groups)
            {
                DependencyList.Items.Add(item);
                MainList.Items.Remove(item);
            }
        }

        private void Remove_Property_Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroup[] groups = DependencyList.SelectedItems.Cast<PropertyGroup>().ToArray();
            foreach (PropertyGroup item in groups)
            {
                MainList.Items.Add(item);
                DependencyList.Items.Remove(item);
            }
        }

        private void MoveItemUp_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Up_Button_Click(null, null, DependencyList.SelectedItems, DependencyList.Items);
        }

        private void MoveItemDown_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Down_Button_Click(null, null, DependencyList.SelectedItems, DependencyList.Items);
        }

        private void MoveItemTop_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Top_Button_Click(null, null, DependencyList.SelectedItems, DependencyList.Items);
        }

        private void MoveItemBottom_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Bottom_Button_Click(null, null, DependencyList.SelectedItems, DependencyList.Items);
        }

        private void EditCollectionRegex_Button_Click(object sender, RoutedEventArgs e)
        {
            CCollectionRegex window = new CCollectionRegex();
            window.PropertyItem = this.PropertyItem;
            window.ShowDialog();
        }

        private void Accept_Button_Click(object sender, RoutedEventArgs e)
        {
            Result = WindowDictionary.Resources.DialogResult.OK;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Result = WindowDictionary.Resources.DialogResult.Cancel;
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Result == WindowDictionary.Resources.DialogResult.None)
                Result = WindowDictionary.Resources.DialogResult.Cancel;
        }

        #endregion
    }
}
