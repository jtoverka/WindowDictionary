using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using WindowDictionary.Resources;

namespace WindowDictionary.Property.Creator
{
    /// <summary>
    /// Interaction logic for PropertyCreator.xaml
    /// </summary>
    public partial class PropertyCreator : Window, INotifyPropertyChanged, IPropertyControl
    {
        #region Fields

        private bool changedState;
        private string filename;
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
                
                OnPropertyChanged("SelectedPropertyGroup");
            }
        }

        /// <summary>
        /// Gets the collection of <see cref="PropertyGroup"/> objects.
        /// </summary>
        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public PropertyCreator()
        {
            DataContext = this;
            InitializeComponent();
            
            WindowControl.Window = this;
            WindowControl.WindowExit += WindowControl_WindowExit;
            
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

        private void WindowControl_WindowExit(object sender, EventArgs e)
        {
            CloseApplication();
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
            if (!changedState)
                return true;

            MessageBoxResult messageBoxResult = MessageBox.Show("Do you want to save your progress?", "Continue Confirmation", MessageBoxButton.YesNoCancel);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                Save_Click(null, null);

                return !changedState;
            }

            return true;
        }

        private void Menu_Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (WindowState ==  WindowState.Maximized)
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

        /// <summary>
        /// Create a new unsaved file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void New_File_Click(object sender, RoutedEventArgs e)
        {
            if (!ContinueChange())
                return;

            try
            {
                this.filename = null;

                this.PropertyGroups.Clear();

                PropertyGroup parent = Initialize.Group("");
                PropertyGroup group = parent.PropertyGroups[0];
                group.Title = "Master List";
                group.Parent = this;

                this.PropertyGroups.Add(group);

                changedState = true;
            }
            catch
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

        /// <summary>
        /// Read XML file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Open standard file
        /// </summary>
        /// <param name="filename"></param>
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
                catch
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
                catch
                {
                    file.Close();
                    changedState = true;
                }
            }
        }

        #endregion
    }
}
