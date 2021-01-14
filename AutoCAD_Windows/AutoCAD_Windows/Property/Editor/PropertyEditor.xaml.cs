using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using WindowDictionary.Resources;

namespace WindowDictionary.Property.Editor
{
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : Window, INotifyPropertyChanged, IPropertyControl
    {
        #region Fields

        private bool changedState;
        private string filename;
        private PropertyGroup _SelectedPropertyGroup;
        private ListView _PropertyItems;
        private System.Windows.Forms.DialogResult _Result = System.Windows.Forms.DialogResult.None;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the root path
        /// </summary>
        public string Path { get; } = "";

        /// <summary>
        /// Property Group Selected under groups
        /// </summary>
        public PropertyGroup SelectedPropertyGroup
        {
            get { return this._SelectedPropertyGroup; }
            set
            {
                if (this._SelectedPropertyGroup == value)
                    return;

                this._SelectedPropertyGroup = value;

                OnPropertyChanged("SelectedPropertyGroup");
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="PropertyGroup"/> objects.
        /// </summary>
        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();

        /// <summary>
        /// 
        /// </summary>
        public ListView PropertyItems
        {
            get { return _PropertyItems; }
            set
            {
                if (_PropertyItems == value)
                    return;

                _PropertyItems = value;
                OnPropertyChanged("PropertyItems");
            }
        }

        /// <summary>
        /// The result of the dialog box
        /// </summary>
        public System.Windows.Forms.DialogResult Result
        {
            get { return this._Result; }
            set
            {
                if (this._Result == value)
                    return;

                this._Result = value;
                OnPropertyChanged("Result");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        /// <param name="filename"></param>
        public PropertyEditor(string filename)
        {
            DataContext = this;

            InitializeComponent();

            WindowControl.Window = this;
            WindowControl.WindowExit += WindowControl_WindowExit;

            Open_File(filename);

            changedState = false;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.OK;
            this.CloseApplication();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.Cancel;
            this.CloseApplication();
        }

        #endregion

        #region Delegates, Events, Handlers


        /// <summary>
        /// Operation to perform when closing the application
        /// </summary>
        private void CloseApplication()
        {
            if (!ContinueChange())
                return;

            this.Close();
        }

        /// <summary>
        /// Check if you wish to continue
        /// </summary>
        /// <returns></returns>
        private bool ContinueChange()
        {
            if (changedState)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("Are you sure you want to cancel?", "Cancel Confirmation", MessageBoxButton.YesNoCancel);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    if (changedState)
                        return false;
                }

                else if (messageBoxResult == MessageBoxResult.Cancel)
                    return false;
            }
            return true;
        }

        private void Menu_Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    var point = PointToScreen(e.MouseDevice.GetPosition(this));

                    Left = point.X - (RestoreBounds.Width * 0.5);
                    Top = point.Y;

                    WindowState = WindowState.Normal;
                }

                this.DragMove();
            }
        }

        /// <summary>
        /// Window minimize button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedPropertyGroup = Tree.SelectedItem as PropertyGroup;
        }

        #endregion

        /// <summary>
        /// Converts an xml file to a list of properties for editing
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static ObservableCollection<PropertyGroup> Convert(string filename)
        {
            //PropertyGroup MasterList = Read_File(filename)[0];

            return GetPropertyGroups(null);//MasterList.PropertyGroups);
        }

        /// <summary>
        /// Gets the property items from the properties group
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static ObservableCollection<PropertyItem> GetPropertyItems(ObservableCollection<PropertyGroup> groups)
        {
            ObservableCollection<PropertyItem> propertyItems = new ObservableCollection<PropertyItem>();

            foreach (PropertyGroup group in groups)
            {
                PropertyItem propertyItem = new PropertyItem();

                foreach (var item in group.PropertyItems[3].Values)
                {
                    propertyItem.Values.Add(item);
                }
                propertyItems.Add(propertyItem);
            }

            return propertyItems;
        }
        /// <summary>
        /// Gets the property groups from the groups collection
        /// </summary>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static ObservableCollection<PropertyGroup> GetPropertyGroups(ObservableCollection<PropertyGroup> groups)
        {
            ObservableCollection<PropertyGroup> propertyGroup = new ObservableCollection<PropertyGroup>();

            foreach (PropertyGroup group in groups)
            {
                PropertyGroup property = new PropertyGroup()
                {
                    Title = group.Title,
                };
                foreach (PropertyItem item in GetPropertyItems(group.PropertyGroups[1].PropertyGroups))
                {
                    property.PropertyItems.Add(item);
                }
                foreach (PropertyGroup item in GetPropertyGroups(group.PropertyGroups[0].PropertyGroups))
                {
                    property.PropertyGroups.Add(item);
                }
                propertyGroup.Add(property);
            }
            return propertyGroup;
        }

        private ObservableCollection<PropertyGroup> Read_File(string filename)
        {
            ObservableCollection<PropertyGroup> groups = new ObservableCollection<PropertyGroup>();

            using var file = new FileStream(filename, FileMode.Open);

            var serializer = new XmlSerializer(typeof(ObservableCollection<PropertyGroup>));
            var collection = serializer.Deserialize(file) as ObservableCollection<PropertyGroup>;

            file.Close();

            foreach (PropertyGroup item in collection)
                groups.Add(item);

            return groups;
        }

        private void Open_File(string filename)
        {
            if (filename != null && filename != "" && File.Exists(filename))
            {
                this.filename = filename;

                try
                {
                    foreach (PropertyGroup item in Read_File(this.filename))
                    {
                        item.Parent = this;
                        this.PropertyGroups.Add(item);
                    }

                    changedState = false;
                }
                catch (Exception)
                {
                    changedState = true;
                    MessageBox.Show("Invalid File");
                }
            }
        }

        private void WindowControl_WindowExit(object sender, EventArgs e)
        {
            this.CloseApplication();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PropertyGroup> GetProperties()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsPropertyGroupUnique(string name)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetTreePath()
        {
            throw new NotImplementedException();
        }
    }
}