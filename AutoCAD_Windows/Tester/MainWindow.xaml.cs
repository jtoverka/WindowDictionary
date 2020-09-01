using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

            var window = new PropertyCreator();


            var dialog = new VistaOpenFileDialog()
            {
                CheckPathExists = true,
                AddExtension = true,
                Filter = "XML File (*.xml)|*.xml",
            };
            dialog.ShowDialog();

            // var property = new PropertyEditor();
            var test = PropertyCreator.Read_File(dialog.FileName);
            _ = test;


            // window.ShowDialog();
        }

        private ObservableCollection<PropertyGroup> Convert (ObservableCollection<PropertyGroup> groups)
        {
            var converted = new ObservableCollection<PropertyGroup>();

            foreach (PropertyGroup item in groups[0].PropertyGroups)
            {
                /*

                public void convert()
   {
     var converted = new ObservableCollection<PropertyGroup>();
     PropertyGroup MasterList = fileload[0];

     foreach (PropertyGroup item in MasterList.PropertyGroups)
     {
       converted.Add(processGroup(item));
     }
   }

   public PropertyGroup processGroup(PropertyGroup group)
   {
     var newGroup = new PropertyGroup();

     // Check if group
     if (group.PropertyItems.Count == 2)
     {
       foreach (var item in group.PropertyGroups)
       {
         newGroup.PropertyGroups.Add(processGroup(item));
       }
       return newGroup;
     }

     // Check if Property
     if (group.PropertyItems.Count == 5)
     {
       newGroup.Title = group.PropertyItems[1].Values[0];

       var property = new PropertyItem()
       {
         PropertyName = group.Title;
         ValueType = (PropertyType)group.PropertyItems[2].ValueIndex;
         ValueRange = group.PropertyItems[4].ValueRange;
       };
       foreach (var item in group.PropertyItems[3].Values)
       {
         property.Values.Add(item)
       }
       newGroup.PropertyItems.Add(property);
       return newGroup;
     }

     // Check if Groups
     if (group.PropertyItems.Count == 1)
     {
       foreach (var item in group.PropertyGroups)
       {
         processGroup(item).PropertyItems[0]
         newGroup.PropertyItems.Add();
       }
       return newGroup
     }

     return null;
   }

                */
            }

            return converted;
        }
    }
}
