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
using WindowDictionary.Property.Creator;
using WindowDictionary.Property.Editor;

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
            window.ShowDialog();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            var dialog = new VistaOpenFileDialog()
            {
                CheckPathExists = true,
                AddExtension = true,
                Filter = "XML File (*.xml)|*.xml",
            };
            dialog.ShowDialog();

            if ((dialog.FileName != null) && (dialog.FileName != ""))
            {
                var window = new PropertyEditor();
                ObservableCollection<PropertyGroup> read = PropertyCreator.Read_File(dialog.FileName);
                foreach (PropertyGroup group in read)
                {
                    foreach (PropertyGroup item in PropertyEditor.Convert(group).PropertyGroups)
                    {
                        window.PropertyGroups.Add(item);
                    }
                }

                window.ShowDialog();

                if (window.Result != WindowDictionary.Resources.DialogResult.Yes)
                    return;

                _ = window.PropertyGroups;
            }
        }
    }
}
