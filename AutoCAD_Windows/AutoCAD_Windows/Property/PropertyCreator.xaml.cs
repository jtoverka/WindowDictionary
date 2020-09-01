using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using WindowDictionary.Resources;
using WindowDictionary.Property.Logic;
using WindowDictionary.Property.ListViewItems;
using System.Windows.Data;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for PropertyCreator.xaml
    /// </summary>
    public partial class PropertyCreator : Window, INotifyPropertyChanged
    {
        #region Fields

        private bool changedState;
        private string filename;
        private bool mRestoreForDragMove;
        private PropertyGroup _SelectedPropertyGroup;

        #endregion

        #region Properties

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

                PropertyItems = Convert(value);

                OnPropertyChanged("SelectedPropertyGroup");
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="PropertyGroup"/> objects.
        /// </summary>
        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();


        private ListView _PropertyItems;
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

            New_File_Click(null, null);

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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.CloseApplication();
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedPropertyGroup = Tree.SelectedItem as PropertyGroup;
        }

        #endregion

        /// <summary>
        /// Creates a property group with a child property group.
        /// This child property group can add sub-children to itself
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private PropertyGroup InitializeGroup(string title)
        {
            // Create root Group
            var item = new PropertyGroup()
            {
                Title = title,
            };

            #region Root Group Properties

            #region Root Group Delete Property

            // Create Property to allow Group to delete itself
            var deleteProperty = new PropertyItem()
            {
                PropertyName = "Delete Group",
                ValueType = PropertyType.Boolean,
                ValueIndex = 2,
            };
            deleteProperty.Values.Add("Delete");
            item.PropertyItems.Add(deleteProperty);

            #endregion

            #region Root Group Rename Property

            // Allow only alphabetical characters with underscores
            LogicalGate gate = new LogicalGate(LogicalOperator.OR);

            gate.RangeCollection.Add(new CharRange('a', 'z'));
            gate.RangeCollection.Add(new CharRange('A', 'Z'));
            gate.RangeCollection.Add(new CharRange('_', '_'));
            gate.RangeCollection.Add(new CharRange(' ', ' '));
            gate.RangeCollection.Add(new CharRange('0', '9'));

            // Name of property
            var groupName = new PropertyItem()
            {
                PropertyName = "Group Name",
                ValueType = PropertyType.String,
                ValueRange = gate,
            };
            groupName.Values.Add("Group");

            // Add Name property to group
            item.PropertyItems.Add(groupName);

            #endregion

            #endregion

            #region Group Collection

            // Create Group Collection (Group)
            var groups = new PropertyGroup()
            {
                Title = "Groups",
            };

            // Create Property to allow Group to add groups
            var addGroup = new PropertyItem()
            {
                PropertyName = "Add Group",
                ValueType = PropertyType.Boolean,
                ValueIndex = 3,
            };
            addGroup.Values.Add("Add");

            // Add property to Groups collection
            groups.PropertyItems.Add(addGroup);

            // Add Groups collection to root group
            item.PropertyGroups.Add(groups);

            #endregion

            #region Properties Collection (Group)

            // Create Property Collection (Group)
            var properties = new PropertyGroup()
            {
                Title = "Properties",
            };

            // Create Property to allow Group to add properties
            var addProperty = new PropertyItem()
            {
                PropertyName = "Add Property",
                ValueType = PropertyType.Boolean,
                ValueIndex = 4,
            };
            addProperty.Values.Add("Add");

            // Add property to properties collection
            properties.PropertyItems.Add(addProperty);

            // Add Properites collection to root group
            item.PropertyGroups.Add(properties);

            #endregion

            return item;
        }

        /// <summary>
        /// Create a group that can add children
        /// these children are groups that have only properties.
        /// These properties initialize the types of properties allowed 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        private PropertyGroup InitializeProperty(string title)
        {
            // Create Property Group 
            var item = new PropertyGroup()
            {
                Title = title,
            };

            #region Group Delete Property

            // Create Property to allow Group to delete itself
            var deleteProperty = new PropertyItem()
            {
                PropertyName = "Delete Property",
                ValueType = PropertyType.Boolean,
                ValueIndex = 2,
            };
            deleteProperty.Values.Add("Delete");
            item.PropertyItems.Add(deleteProperty);

            #endregion

            #region Group Rename Property

            // Allow only alphabetical characters with underscores
            LogicalGate gate = new LogicalGate(LogicalOperator.OR);

            gate.RangeCollection.Add(new CharRange('a', 'z'));
            gate.RangeCollection.Add(new CharRange('A', 'Z'));
            gate.RangeCollection.Add(new CharRange('_', '_'));
            gate.RangeCollection.Add(new CharRange(' ', ' '));
            gate.RangeCollection.Add(new CharRange('0', '9'));

            // Name of property
            var propertyName = new PropertyItem()
            {
                PropertyName = "Property Name",
                ValueType = PropertyType.String,
                ValueRange = gate,
            };
            propertyName.Values.Add("Property");

            // Add Name property to group
            item.PropertyItems.Add(propertyName);

            #endregion

            #region Group Type Property

            // Type of property
            var propertyType = new PropertyItem()
            {
                PropertyName = "Property Type",
                ValueType = PropertyType.SelectionString,
                ValueIndex = 3,
            };

            // Create a selection of property types to choose from
            var selection = new ObservableCollection<string>(Enum.GetNames(typeof(PropertyType)));

            // Add Selection
            foreach (string select in selection)
            {
                propertyType.Values.Add(select);
            }

            // Add Type of Property to group
            item.PropertyItems.Add(propertyType);

            #endregion

            #region Group Values Property

            // Create Property to add values
            var Value = new PropertyItem()
            {
                PropertyName = "Property Values",
                ValueType = PropertyType.Boolean,
                ValueIndex = 1,
            };

            // Add property to Groups collection
            item.PropertyItems.Add(Value);

            #endregion

            #region Group Range Property

            // Create property to allow range input
            var rangeProperty = new PropertyItem()
            {
                PropertyName = "Range",
                ValueType = PropertyType.Boolean,
                ValueIndex = 5,
                ValueRange = new LogicalGate(),
            };

            item.PropertyItems.Add(rangeProperty);

            #endregion

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

                PropertyGroup parent = InitializeGroup("");
                PropertyGroup group = parent.PropertyGroups[0];
                group.Title = "Master List";
                group.Parent = this;

                this.PropertyGroups.Add(group);

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
        public static ObservableCollection<PropertyGroup> Read_File(string filename)
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
                    foreach (PropertyGroup item in Read_File(filename))
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
                catch //(Exception exception)
                {
                    file.Close();
                    changedState = true;
                }
            }
        }

        private void TextBox_Double_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewKeyDown(sender, e);
        }

        private void TextBox_Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewTextInput(sender, e);

            if (e.Handled == true)
                return;

            TextBox textbox = sender as TextBox;
            ListViewItem listviewitem = textbox.DataContext as ListViewItem;
            PropertyItem Item = listviewitem.Tag as PropertyItem;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            if (!Item.ValueRange.IsValid(text))
            {
                e.Handled = true;

            }
        }

        private void TextBox_Integer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Integer_PreviewKeyDown(sender, e);
        }

        private void TextBox_Integer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_String_PreviewTextInput(sender, e);

            if (e.Handled == true)
                return;

            TextBox textbox = sender as TextBox;
            ListViewItem listviewitem = textbox.DataContext as ListViewItem;
            PropertyItem Item = listviewitem.Tag as PropertyItem;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            if (!Item.ValueRange.IsValid(text))
            {
                e.Handled = true;

            }
        }

        private void Up_Button_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            var valueList = control.DataContext as LVI_ValueList;

            PropertyItem item = SelectedPropertyGroup.PropertyItems[3];
            
            UILibrary.Up_Button_Click(sender, e, valueList.ListView.SelectedItems, item.Values);
        }
        private void Down_Button_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            var valueList = control.DataContext as LVI_ValueList;

            PropertyItem item = SelectedPropertyGroup.PropertyItems[3];

            UILibrary.Down_Button_Click(sender, e, valueList.ListView.SelectedItems, item.Values);
        }
        private void Top_Button_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            var valueList = control.DataContext as LVI_ValueList;

            PropertyItem item = SelectedPropertyGroup.PropertyItems[3];

            UILibrary.Top_Button_Click(sender, e, valueList.ListView.SelectedItems, item.Values);
        }
        private void Bottom_Button_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            var valueList = control.DataContext as LVI_ValueList;

            PropertyItem item = SelectedPropertyGroup.PropertyItems[3];

            UILibrary.Bottom_Button_Click(sender, e, valueList.ListView.SelectedItems, item.Values);
        }
        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            var control = sender as Control;
            var valueList = control.DataContext as LVI_ValueList;

            PropertyItem item = SelectedPropertyGroup.PropertyItems[3];

            UILibrary.Remove_Button_Click(sender, e, valueList.ListView.SelectedItems, item.Values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddGroup_Button_Click(object sender, RoutedEventArgs e)
        {
            SelectedPropertyGroup.PropertyGroups.Add(InitializeGroup("Group"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteGroup_Button_Click(object sender, RoutedEventArgs e)
        {
            PropertyGroup group = SelectedPropertyGroup.Parent as PropertyGroup;

            group.PropertyGroups.Remove(SelectedPropertyGroup);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProperty_Button_Click (object sender, RoutedEventArgs e)
        {
            SelectedPropertyGroup.PropertyGroups.Add(InitializeProperty("Property"));
        }

        /// <summary>
        /// Rename a group text input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Rename_Text_Input (object sender, TextChangedEventArgs e)
        {
            var textbox = sender as TextBox;

            SelectedPropertyGroup.Title = textbox.Text;
        }

        /// <summary>
        /// Add Button is clicked to add value to values collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Add_Value_Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Control;
            var element = button.DataContext as LVI_ValueList;
            var property = element.Tag as PropertyItem;

            if (property.Values.Contains(element.TextBox.Text))
            {
                //element.TriggerPopup("No Duplicates are allowed!");
            }
            else
            if (element.TextBox.Text == "")
            {
                //element.TriggerPopup("Null values not allowed!");
            }
            else
            {
                property.Values.Add(element.TextBox.Text);
                element.TextBox.Text = "";
            }
        }

        /// <summary>
        /// Trigger TextBox_PreviewKeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var element = sender as TextBox;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;

                Add_Value_Button_Click(sender, null);

                element.Focus();
            }
        }

        /// <summary>
        /// Trigger TextBox_PreviewTextInput event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textbox = sender as TextBox;
            var listItem = textbox.DataContext as ListViewItem;

            PropertyItem item = listItem.Tag as PropertyItem;
            var parent = item.Parent as PropertyGroup;
            int index = parent.PropertyItems[2].ValueIndex;
            
            bool allowed = false;
            string errorMessage = "";

            switch ((PropertyType)index)
            {
                case PropertyType.Boolean:
                    allowed = false;
                    errorMessage = "No values allowed for boolean!";
                    break;
                case PropertyType.Double:
                    allowed = UILibrary.IsTextAllowed(textbox.Text, TypeCode.Double);
                    errorMessage = "The Value needs to be a double!";
                    break;
                case PropertyType.Integer:
                    allowed = UILibrary.IsTextAllowed(textbox.Text, TypeCode.Int32);
                    errorMessage = "The Value needs to be an Integer (32 bit)!";
                    break;
                case PropertyType.SelectionString:
                    allowed = true;
                    break;
                case PropertyType.SelectionEditDouble:
                    allowed = UILibrary.IsTextAllowed(textbox.Text, TypeCode.Double);
                    errorMessage = "The Value needs to be a double!";
                    break;
                case PropertyType.SelectionEditInteger:
                    allowed = UILibrary.IsTextAllowed(textbox.Text, TypeCode.Int32);
                    errorMessage = "The Value needs to be an Integer (32 bit)!";
                    break;
                case PropertyType.SelectionEditString:
                    allowed = true;
                    break;
                case PropertyType.String:
                    allowed = true;
                    break;
                default:
                    break;
            }

            if (!allowed)
            {
                e.Handled = true;
                _ = errorMessage;
                //element.TriggerPopup(errorMessage);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private ListView Convert(object value)
        {
            if (value == null)
                return null;

            var itemParent = value as PropertyGroup;
            var propertyItems = itemParent.PropertyItems;
            var list = new ListView();

            foreach (PropertyItem item in propertyItems)
            {
                int itemValueIndex = item.ValueIndex;
                Range range = item.ValueRange;
                PropertyType itemValueType = item.ValueType;
                string itemPropertyName = item.PropertyName;

                var PropertyNameBinding = new Binding("PropertyName") { Source = item };
                var ValueZeroBinding = new Binding("Values[0]") { Source = item };
                var ValueIndexBinding = new Binding("ValueIndex") { Source = item };
                var ValuesBinding = new Binding("Values") { Source = item };

                switch (item.ValueType)
                {
                    case PropertyType.Boolean:
                        if (itemValueIndex == 5)
                        {
                            var lvi_range = new LVI_Range();

                            lvi_range.Root.Add(item.ValueRange);

                            list.Items.Add(lvi_range);
                        }
                        else
                        if (itemValueIndex == 4)
                        {
                            var button = new LVI_Button();

                            button.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                            button.Button.SetBinding(Button.ContentProperty, ValueZeroBinding);
                            button.Tag = item;

                            button.Button.Click += AddProperty_Button_Click;
                            list.Items.Add(button);
                        }
                        else
                        if (itemValueIndex == 3)
                        {
                            var button = new LVI_Button();

                            button.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                            button.Button.SetBinding(Button.ContentProperty, ValueZeroBinding);
                            button.Tag = item;

                            button.Button.Click += AddGroup_Button_Click;
                            list.Items.Add(button);
                        }
                        else
                        if (itemValueIndex == 2)
                        {
                            var button = new LVI_Button();

                            button.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                            button.Button.SetBinding(Button.ContentProperty, ValueZeroBinding);
                            button.Tag = item;

                            button.Button.Click += DeleteGroup_Button_Click;
                            list.Items.Add(button);
                        }
                        else
                        if (itemValueIndex == 1)
                        {
                            var listViewSelection = new LVI_ValueList();
                            listViewSelection.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                            listViewSelection.ListView.SetBinding(ItemsControl.ItemsSourceProperty, ValuesBinding);
                            listViewSelection.TextBox.PreviewKeyDown += Text_PreviewKeyDown;
                            listViewSelection.TextBox.PreviewTextInput += Text_PreviewTextInput;
                            listViewSelection.AddButton.Click += Add_Value_Button_Click;
                            listViewSelection.RemoveButton.Click += Remove_Button_Click;
                            listViewSelection.TopButton.Click += Top_Button_Click;
                            listViewSelection.BottomButton.Click += Bottom_Button_Click;
                            listViewSelection.UpButton.Click += Up_Button_Click;
                            listViewSelection.DownButton.Click += Down_Button_Click;
                            listViewSelection.Tag = item;
                            list.Items.Add(listViewSelection);
                        }
                        else
                        if (itemValueIndex == 0)
                        {
                            var checkbox = new LVI_CheckBox();
                            
                            checkbox.CheckBox.SetBinding(CheckBox.IsCheckedProperty, ValueZeroBinding);
                            checkbox.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                            checkbox.Tag = item;

                            list.Items.Add(checkbox);
                        }
                        break;
                    case PropertyType.Double:
                        var textBoxDouble = new LVI_TextBox();
                       
                        textBoxDouble.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        textBoxDouble.TextBox.SetBinding(TextBox.TextProperty, ValueZeroBinding);
                        textBoxDouble.TextBox.PreviewKeyDown += TextBox_Double_PreviewKeyDown;
                        textBoxDouble.TextBox.PreviewTextInput += TextBox_Double_PreviewTextInput;
                        textBoxDouble.Tag = item;

                        list.Items.Add(textBoxDouble);
                        break;
                    case PropertyType.Integer:
                        var textBoxInteger = new LVI_TextBox();
                        
                        textBoxInteger.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        textBoxInteger.TextBox.SetBinding(TextBox.TextProperty, ValueZeroBinding);
                        textBoxInteger.TextBox.PreviewKeyDown += TextBox_Integer_PreviewKeyDown;
                        textBoxInteger.TextBox.PreviewTextInput += TextBox_Integer_PreviewTextInput;
                        textBoxInteger.Tag = item;

                        list.Items.Add(textBoxInteger);
                        break;
                    case PropertyType.SelectionString:
                        var ComboString = new LVI_ComboBox();

                        ComboString.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        ComboString.ComboBox.SetBinding(ComboBox.SelectedIndexProperty, ValueIndexBinding);
                        ComboString.ComboBox.SetBinding(ComboBox.ItemsSourceProperty, ValuesBinding);
                        ComboString.Tag = item;

                        list.Items.Add(ComboString);
                        break;
                    case PropertyType.SelectionEditDouble:
                        var ComboEditDouble = new LVI_ComboBox();

                        ComboEditDouble.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        ComboEditDouble.ComboBox.SetBinding(ComboBox.SelectedValueProperty, ValueIndexBinding);
                        ComboEditDouble.ComboBox.SetBinding(ComboBox.ItemsSourceProperty, ValuesBinding);
                        ComboEditDouble.ComboBox.IsEditable = true;
                        ComboEditDouble.Tag = item;

                        list.Items.Add(ComboEditDouble);
                        break;
                    case PropertyType.SelectionEditInteger:
                        var ComboEditInteger = new LVI_ComboBox();

                        ComboEditInteger.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        ComboEditInteger.ComboBox.SetBinding(ComboBox.SelectedValueProperty, ValueIndexBinding);
                        ComboEditInteger.ComboBox.SetBinding(ComboBox.ItemsSourceProperty, ValuesBinding);
                        ComboEditInteger.ComboBox.IsEditable = true;
                        ComboEditInteger.Tag = item;

                        list.Items.Add(ComboEditInteger);
                        break;
                    case PropertyType.SelectionEditString:
                        var ComboEditString = new LVI_ComboBox();

                        ComboEditString.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        ComboEditString.ComboBox.SetBinding(ComboBox.SelectedValueProperty, ValueIndexBinding);
                        ComboEditString.ComboBox.SetBinding(ComboBox.ItemsSourceProperty, ValuesBinding);
                        ComboEditString.ComboBox.IsEditable = true;
                        ComboEditString.Tag = item;

                        list.Items.Add(ComboEditString);
                        break;
                    case PropertyType.String:
                        var TextBoxString = new LVI_TextBox();

                        TextBoxString.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        TextBoxString.TextBox.SetBinding(TextBox.TextProperty, ValueZeroBinding);
                        TextBoxString.Tag = item;
                        
                        if (itemValueIndex == 0)
                            TextBoxString.TextBox.TextChanged += Rename_Text_Input;
                        
                        list.Items.Add(TextBoxString);
                        break;
                }
            }

            return list;
        }
    }
}
