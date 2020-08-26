using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowDictionary.Property;

namespace Tester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var window = new WindowDictionary.Property.PropertyCreator();

            //var propertyGroup = new PropertyGroup() { Title = "Parent 1" };
            //propertyGroup.PropertyGroups.Add(new PropertyGroup() { Title = "Child 1" });
            //propertyGroup.PropertyGroups.Add(new PropertyGroup() { Title = "Child 2" });
            //propertyGroup.PropertyGroups.Add(new PropertyGroup() { Title = "Child 3" });

            //var childPropertyGroup = new PropertyGroup() { Title = "Child 4" };
            //var subChild = new PropertyGroup() { Title = "Sub Child 1" };
            //subChild.PropertyItems.Add(new PropertyItem(new DoubleRange(0, 5), PropertyType.Double, 2.2) { PropertyName = "Hello World1" });
            //subChild.PropertyItems.Add(new PropertyItem(new IntegerRange(0, 1000), PropertyType.Integer, 10) { PropertyName = "Hello World2" });

            //childPropertyGroup.PropertyGroups.Add(subChild);
            //propertyGroup.PropertyGroups.Add(childPropertyGroup);
            //window.PropertyGroups.Add(propertyGroup);

            //var stringRange = new LogicalGate(LogicalOperator.OR);

            //stringRange.RangeCollection.Add(new CharRange(' ', ' '));
            //stringRange.RangeCollection.Add(new CharRange('0', '9'));
            //stringRange.RangeCollection.Add(new CharRange('A', 'Z'));
            //stringRange.RangeCollection.Add(new CharRange('a', 'z'));

            //subChild = new PropertyGroup() { Title = "Sub Child 2a" };

            //propertyGroup = new PropertyGroup() { Title = "Parent 2" };
            //propertyGroup.PropertyItems.Add(new PropertyItem(stringRange, PropertyType.String, "Hannibal the cannibal") { PropertyName = "Hello World3" });
            //propertyGroup.PropertyGroups.Add(subChild);

            //window.PropertyGroups.Add(propertyGroup);
            //window.PropertyGroups.Add(new PropertyGroup() { Title = "Parent 3" });


            //var window = new VistaFolderBrowserDialog();
            
            window.ShowDialog();
        }
    }
}
