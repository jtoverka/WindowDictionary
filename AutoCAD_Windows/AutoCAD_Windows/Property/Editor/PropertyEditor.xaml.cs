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
using WindowDictionary.Property.Creator;

namespace WindowDictionary.Property.Editor
{
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : Window, INotifyPropertyChanged, IPropertyControl
    {
        #region Fields

        private PropertyGroup _SelectedPropertyGroup;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="DialogResult">Result</see> of this dialog box
        /// </summary>
        public DialogResult Result { get; set; } = WindowDictionary.Resources.DialogResult.None;

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
        /// Gets the root path
        /// </summary>
        public string Path { get; } = "";

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public PropertyEditor()
        {
            DataContext = this;
            InitializeComponent();

            WindowControl.Window = this;
            WindowControl.WindowExit += WindowControl_WindowExit;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get all property groups within this tree
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PropertyGroup> GetProperties()
        {
            ObservableCollection<PropertyGroup> properties = new ObservableCollection<PropertyGroup>();
            foreach (PropertyGroup group in this.PropertyGroups)
            {
                foreach (PropertyGroup item in CollectGroups(group))
                {
                    if (item.PropertyItems[0].Type.CPropertyType == CPropertyType.CProperty)
                    {
                        properties.Add(item);
                    }
                }
            }

            return properties;
        }

        /// <summary>
        /// Collect all sub-groups within a group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        private ObservableCollection<PropertyGroup> CollectGroups(PropertyGroup group)
        {
            ObservableCollection<PropertyGroup> groups = new ObservableCollection<PropertyGroup>();

            foreach (PropertyGroup subGroup in group.PropertyGroups)
            {
                foreach (PropertyGroup item in CollectGroups(subGroup))
                {
                    groups.Add(item);
                }
            }
            groups.Add(group);

            return groups;
        }

        /// <summary>
        /// Determines if the name provided is a unique name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsPropertyGroupUnique(string name)
        {
            foreach (PropertyGroup item in this.PropertyGroups)
            {
                if (item.Title == name)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Convert a file read from <see cref="PropertyCreator.Read_File(string)"/> to be used by the property editor
        /// </summary>
        /// <param name="ParentCollection"></param>
        /// <returns></returns>
        public static PropertyGroup Convert(PropertyGroup ParentCollection)
        {
            PropertyGroup ConvertedCollection = new PropertyGroup()
            {
                Parent = ParentCollection.Parent,
                Title = ParentCollection.Title,
            };
            foreach (PropertyGroup Group in ParentCollection.PropertyGroups)
            {
                if (Group.PropertyItems[0].Type.CPropertyType == CPropertyType.CGroup)
                {
                    PropertyGroup ConvertedGroup = new PropertyGroup()
                    {
                        Parent = ConvertedCollection,
                        Title = Group.Title,
                    };
                    // Get groups from groups collection
                    if (Group.PropertyGroups[0].PropertyGroups.Count > 0)
                    {
                        foreach (PropertyGroup SubGroup in Convert(Group.PropertyGroups[0]).PropertyGroups)
                        {
                            ConvertedGroup.PropertyGroups.Add(SubGroup);
                        }
                    }
                    // Get properties from properties collection
                    foreach (PropertyGroup property in Group.PropertyGroups[1].PropertyGroups)
                    {
                        ConvertedGroup.PropertyItems.Add(property.PropertyItems[0].Clone() as PropertyItem);
                    }
                    ConvertedCollection.PropertyGroups.Add(ConvertedGroup);
                }
            }
            return ConvertedCollection;
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

        private void WindowControl_WindowExit(object sender, EventArgs e)
        {
            if (Result == WindowDictionary.Resources.DialogResult.None)
                Result = WindowDictionary.Resources.DialogResult.Cancel;

            this.Close();
        }

        private void Menu_Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (WindowState == WindowState.Maximized)
                {
                    var point = PointToScreen(e.MouseDevice.GetPosition(this));

                    Left = point.X - (RestoreBounds.Width * 0.5);
                    Top = point.Y;

                    WindowState = WindowState.Normal;
                }

                this.DragMove();
            }
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedPropertyGroup = Tree.SelectedItem as PropertyGroup;
        }

        private void OK_Button_Click(object sender, RoutedEventArgs e)
        {
            Result = WindowDictionary.Resources.DialogResult.OK;
            this.Close();
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Result = WindowDictionary.Resources.DialogResult.Cancel;
            this.Close();
        }

        #endregion
    }
}