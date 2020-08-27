using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for PropertyCreator.xaml
    /// </summary>
    public partial class PropertyCreator : Window, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Keeps track if the current program needs to be saved
        /// </summary>
        private bool changedState;

        /// <summary>
        /// The filename the MainWindow is operating on
        /// </summary>
        private string filename;

        /// <summary>
        /// Keeps track of drag and move from maximized to normal window views
        /// </summary>
        public bool mRestoreForDragMove;

        #endregion

        #region Properties

        private PropertyGroup _SelectedPropertyGroup;
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
        /// Collection of <see cref="PropertyGroup">Property Groups</see>
        /// </summary>
        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();

        private System.Windows.Forms.DialogResult _Result = System.Windows.Forms.DialogResult.None;
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
        public PropertyCreator()
        {
            DataContext = this;

            InitializeComponent();

            // Set window state
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        this.Window_Resize_Button.ToolTip = "Maximize";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/maximizeAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(0);
                        break;
                    }
                case WindowState.Maximized:
                    {
                        this.Window_Resize_Button.ToolTip = "Restore Down";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/toNormalAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(7);
                        break;
                    }
            }

            PropertyGroup parent = InitializeGroup("Master List");

            this.PropertyGroups.Add(parent);

            changedState = true;
        }

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        /// <param name="filename"></param>
        public PropertyCreator(string filename)
        {
            DataContext = this;

            InitializeComponent();

            // Set window state
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        this.Window_Resize_Button.ToolTip = "Maximize";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/maximizeAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(0);
                        break;
                    }
                case WindowState.Maximized:
                    {
                        this.Window_Resize_Button.ToolTip = "Restore Down";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/toNormalAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(7);
                        break;
                    }
            }

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
                MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Do you want to save your progress?", "Continue Confirmation", System.Windows.MessageBoxButton.YesNoCancel);
                if (messageBoxResult == MessageBoxResult.Yes)
                {
                    Save_Click(null, null);

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
                if (this.mRestoreForDragMove)
                {
                    this.mRestoreForDragMove = false;

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

        /// <summary>
        /// Window resize button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Resize_Click(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    this.Window_Resize_Button.ToolTip = "Maximize";
                    this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/maximizeAppIcon.ico", UriKind.Relative));
                    this.BorderThickness = new Thickness(0);
                    break;
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    this.Window_Resize_Button.ToolTip = "Restore Down";
                    this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/toNormalAppIcon.ico", UriKind.Relative));
                    this.BorderThickness = new Thickness(7);
                    break;
            }
        }

        /// <summary>
        /// Change Window State Icon when maximized or minimized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        this.Window_Resize_Button.ToolTip = "Maximize";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/maximizeAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(0);
                        this.mRestoreForDragMove = false;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        this.Window_Resize_Button.ToolTip = "Restore Down";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"../Application/toNormalAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(7);
                        this.mRestoreForDragMove = true;
                        break;
                    }
            }
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

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedPropertyGroup = Tree.SelectedItem as PropertyGroup;
        }

        #endregion

        private PropertyGroup InitializeGroup(string title)
        {
            var item = new PropertyGroup()
            {
                Title = title,
            };

            var groups = new PropertyGroup()
            {
                Title = "Groups",
            };
            var addGroup = new PropertyItem()
            {
                PropertyName = "Add Group",
                ValueType = PropertyType.Button,
                Value = "Add",
                EventHandler = new RoutedEventHandler(AddGroup_Button_Click),
            };

            var properties = new PropertyGroup()
            {
                Title = "Properties",
            };
            var addProperty = new PropertyItem()
            {
                PropertyName = "Add Property",
                ValueType = PropertyType.Button,
                Value = "Add",
                EventHandler = new RoutedEventHandler(AddProperty_Button_Click),
            };

            groups.PropertyItems.Add(addGroup);
            properties.PropertyItems.Add(addProperty);

            item.PropertyGroups.Add(groups);
            item.PropertyGroups.Add(properties);

            return item;
        }

        private PropertyGroup InitializeProperty(string title)
        {
            var item = new PropertyGroup()
            {
                Title = title,
            };

            LogicalGate gate = new LogicalGate(LogicalOperator.OR);

            gate.RangeCollection.Add(new CharRange('a', 'z'));
            gate.RangeCollection.Add(new CharRange('A', 'Z'));
            gate.RangeCollection.Add(new CharRange('_', '_'));

            var propertyName = new PropertyItem()
            {
                PropertyName = "Property Name",
                ValueType = PropertyType.String,
                ValueRange = gate,
                Value = "Property"
            };

            var selection = new ObservableCollection<string>(Enum.GetNames(typeof(PropertyType)));

            var propertyType = new PropertyItem()
            {
                PropertyName = "Property Type",
                ValueType = PropertyType.SelectionString,
                Value = selection[0],
            };
            foreach (string select in selection)
            {
                propertyType.ValueSelection.Add(select);
            }

            item.PropertyItems.Add(propertyName);
            item.PropertyItems.Add(propertyType);

            return item;
        }

        private void New_File_Click(object sender, RoutedEventArgs e)
        {
            if (!ContinueChange())
                return;

            try
            {
                this.filename = null;

                this.PropertyGroups.Clear();

                PropertyGroup parent = InitializeGroup("Master List");

                this.PropertyGroups.Add(parent);

                changedState = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Unhandled Exception");
            }

            changedState = true;
        }

        /// <summary>
        /// Open existing standard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Open_File_Click(object sender, RoutedEventArgs e)
        {
            if (!ContinueChange())
                return;

            using var dialog = new System.Windows.Forms.OpenFileDialog()
            {
                CheckPathExists = true,
                AddExtension = true,
                Filter = "XML File (*.xml)|*.xml",
            };
            dialog.ShowDialog();
            
            Open_File(dialog.FileName);
        }

        private void Open_File(string filename)
        {
            if (filename != null && filename != "" && File.Exists(filename))
            {
                this.filename = filename;
                using var file = new FileStream(this.filename, FileMode.Open);

                try
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<PropertyGroup>));
                    var collection = serializer.Deserialize(file) as ObservableCollection<PropertyGroup>;

                    file.Close();

                    this.PropertyGroups.Clear();
                    foreach (PropertyGroup item in collection)
                    {
                        FillHandlers(item);
                        this.PropertyGroups.Add(item);
                    }

                    changedState = false;
                }
                catch (Exception)
                {
                    file.Close();
                    changedState = true;
                    MessageBox.Show("Invalid File");
                }
            }
        }

        private void FillHandlers(PropertyGroup propertyGroup)
        {
            foreach (PropertyGroup group in propertyGroup.PropertyGroups)
            {
                FillHandlers(group);
            }
            foreach (PropertyItem propertyItem in propertyGroup.PropertyItems)
            {
                FillHandlers(propertyItem);
            }
        }
        private void FillHandlers(PropertyItem propertyItem)
        {
            if (propertyItem.PropertyName == "Add Property")
                propertyItem.EventHandler = new RoutedEventHandler(AddProperty_Button_Click);
            else if (propertyItem.PropertyName == "Add Group")
                propertyItem.EventHandler = new RoutedEventHandler(AddGroup_Button_Click);
        }

        /// <summary>
        /// Save the current standard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (this.filename != null && this.filename != "")
            {
                using var file = new FileStream(this.filename, FileMode.Create);

                try
                {
                    var serializer = new XmlSerializer(typeof(ObservableCollection<PropertyGroup>));
                    serializer.Serialize(file, this.PropertyGroups);
                    file.Close();

                    changedState = false;
                }
                catch (Exception)
                {
                    file.Close();
                    changedState = true;
                }
            }
            else
            {
                Save_As_Click(sender, e);
            }
        }

        /// <summary>
        /// Save as the current standard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_As_Click(object sender, RoutedEventArgs e)
        {
            using var dialog = new System.Windows.Forms.SaveFileDialog()
            {
                CheckPathExists = true,
                AddExtension = true,
                Filter = "XML File (*.xml)|*.xml",
            };
            dialog.ShowDialog();
            if (dialog.FileName != null && dialog.FileName != "" && dialog.CheckPathExists)
            {
                this.filename = dialog.FileName;
                using var file = new FileStream(this.filename, FileMode.Create);

                try
                {
                    var serializer = new XmlSerializer(this.PropertyGroups.GetType());
                    serializer.Serialize(file, this.PropertyGroups);
                    file.Close();

                    changedState = false;
                }
                catch (Exception exception)
                {
                    file.Close();
                    changedState = true;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddGroup_Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as ListItemButton;
            
            if (button == null)
                return;

            PropertyGroup group = button.ParentGroup;

            group.PropertyGroups.Add(InitializeGroup("Group"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AddProperty_Button_Click (object sender, RoutedEventArgs e)
        {
            var button = sender as ListItemButton;

            if (button == null)
                return;

            PropertyGroup group = button.ParentGroup;

            group.PropertyGroups.Add(InitializeProperty("Property"));
        }
    }
}
