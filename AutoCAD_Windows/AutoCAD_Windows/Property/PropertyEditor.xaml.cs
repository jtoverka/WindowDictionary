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
using WindowDictionary.Property.ListViewItems;
using WindowDictionary.Property.Logic;
using WindowDictionary.Resources;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : Window, INotifyPropertyChanged
    {
        #region Fields

        private bool changedState;
        private string filename;
        private bool mRestoreForDragMove;
        private PropertyGroup _SelectedPropertyGroup;
        private ListView _PropertyItems;
        private System.Windows.Forms.DialogResult _Result = System.Windows.Forms.DialogResult.None;

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
            PropertyGroup MasterList = PropertyCreator.Read_File(filename)[0];

            return GetPropertyGroups(MasterList.PropertyGroups);
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
                PropertyItem propertyItem = new PropertyItem()
                {
                    PropertyName = group.PropertyItems[1].Values[0].ToString(),
                    ValueType = (PropertyType)group.PropertyItems[2].ValueIndex,
                    ValueRange = group.PropertyItems[4].ValueRange,
                };

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




        /// <summary>
        /// Rename a group text input
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Rename_Text_Input(object sender, TextChangedEventArgs e)
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
                        var checkbox = new LVI_CheckBox();

                        checkbox.CheckBox.SetBinding(CheckBox.IsCheckedProperty, ValueZeroBinding);
                        checkbox.TextBlock.SetBinding(TextBlock.TextProperty, PropertyNameBinding);
                        checkbox.Tag = item;

                        list.Items.Add(checkbox);
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