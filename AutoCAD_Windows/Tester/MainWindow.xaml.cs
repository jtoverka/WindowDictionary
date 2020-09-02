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
            var window = new PropertyEditor();
            //window.ShowDialog();
            
            var dialog = new VistaOpenFileDialog()
            {
                CheckPathExists = true,
                AddExtension = true,
                Filter = "XML File (*.xml)|*.xml",
            };
            dialog.ShowDialog();

            // var property = new PropertyEditor();
            ObservableCollection<PropertyGroup> groups = PropertyEditor.Convert(dialog.FileName);

            foreach (PropertyGroup group in groups)
            {
                window.PropertyGroups.Add(group);
            }
            
            window.ShowDialog();
        }

    }
}
