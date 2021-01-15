using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            set 
            {
                UpdateList(value);
                SetValue(PropertyItemProperty, value); 
            }
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
        public ObservableCollection<object> DependencyItems { get; } = new ObservableCollection<object>();

        #endregion
        #region Property - DependencyItems : ObservableCollection<DependencyItem>

        /// <summary>
        /// Gets the collection of available property items
        /// </summary>
        public ObservableCollection<object> AvailableProperties { get; } = new ObservableCollection<object>();

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
        private void UpdateList(PropertyItem property)
        {
            if (property.Parent == null)
                return;

            this.AvailableProperties.Clear();
            this.DependencyItems.Clear();
            bool skip = false;
            foreach (PropertyGroup item in property.Parent.GetProperties())
            {
                foreach (DependencyItem dependency in property.DependencyItems)
                {
                    if (dependency.Property.Equals(item.Path))
                    {
                        skip = true;
                        break;
                    }
                }
                if (!skip)
                    AvailableProperties.Add(item.Path);

                skip = false;
            }
            foreach (DependencyItem dependency in property.DependencyItems)
            {
                this.DependencyItems.Add(dependency);
            }
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Add_Property_Button_Click(object sender, RoutedEventArgs e)
        {
            string[] groups = MainList.SelectedItems.Cast<string>().ToArray();
            foreach (string item in groups)
            {
                this.DependencyItems.Add(
                    new DependencyItem()
                    {
                        Code = "P",
                        Property = item,
                        Regex = ".*",
                    });
                this.AvailableProperties.Remove(item);
            }
        }

        private void Remove_Property_Button_Click(object sender, RoutedEventArgs e)
        {
            DependencyItem[] groups = DependencyList.SelectedItems.Cast<DependencyItem>().ToArray();
            foreach (DependencyItem item in groups)
            {
                this.AvailableProperties.Add(item.Property);
                this.DependencyItems.Remove(item);
            }
        }

        private void MoveItemUp_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Up_Button_Click(null, null, DependencyList.SelectedItems, this.DependencyItems);
        }

        private void MoveItemDown_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Down_Button_Click(null, null, DependencyList.SelectedItems, this.DependencyItems);
        }

        private void MoveItemTop_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Top_Button_Click(null, null, DependencyList.SelectedItems, this.DependencyItems);
        }

        private void MoveItemBottom_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Bottom_Button_Click(null, null, DependencyList.SelectedItems, this.DependencyItems);
        }

        private void EditCollectionRegex_Button_Click(object sender, RoutedEventArgs e)
        {
            CCollectionRegex window = new CCollectionRegex
            {
                PropertyItem = this.PropertyItem
            };
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

        private void DependencyList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(sender is ListViewItem item))
                return;

            if (!(item.DataContext is DependencyItem dependency))
                return;

            CDependencyRegex window = new CDependencyRegex
            {
                DependencyItem = dependency
            };
            window.ShowDialog();
        }
    }
}
