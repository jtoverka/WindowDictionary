using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : Window, INotifyPropertyChanged
    {

        private PropertyGroup _SelectedPropertyGroup;
        /// <summary>
        /// 
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
        /// 
        /// </summary>
        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();


        private System.Windows.Forms.DialogResult _Result = System.Windows.Forms.DialogResult.None;
        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        public PropertyEditor()
        {
            DataContext = this;

            var propertyGroup = new PropertyGroup() { Title = "Parent 1" };
            propertyGroup.PropertyGroups.Add(new PropertyGroup() { Title = "Child 1" });
            propertyGroup.PropertyGroups.Add(new PropertyGroup() { Title = "Child 2" });
            propertyGroup.PropertyGroups.Add(new PropertyGroup() { Title = "Child 3" });

            var childPropertyGroup = new PropertyGroup() { Title = "Child 4" };
            var subChild = new PropertyGroup() { Title = "Sub Child 1" };
            subChild.PropertyItems.Add(new PropertyItem(new DoubleRange(0, 5), PropertyType.Double, 2.2) { PropertyName = "Hello World1" });
            subChild.PropertyItems.Add(new PropertyItem(new IntegerRange(0, 1000), PropertyType.Integer, 10) { PropertyName = "Hello World2" });
            childPropertyGroup.PropertyGroups.Add(subChild);
            propertyGroup.PropertyGroups.Add(childPropertyGroup);

            PropertyGroups.Add(propertyGroup);
            PropertyGroups.Add(new PropertyGroup() { Title = "Parent 2" });
            PropertyGroups.Add(new PropertyGroup() { Title = "Parent 3" });

            InitializeComponent();
        }

        /// <summary>
        /// 
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
    }
}
