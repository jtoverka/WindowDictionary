using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : Window, INotifyPropertyChanged
    {
        #region Fields

        private bool mRestoreForDragMove;

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
        public PropertyEditor()
        {
            DataContext = this;

            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        /// <param name="filename"></param>
        public PropertyEditor(string filename)
        {
            DataContext = this;

            InitializeComponent();
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

        private void CloseApplication()
        {
            this.Close();
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
    }
}