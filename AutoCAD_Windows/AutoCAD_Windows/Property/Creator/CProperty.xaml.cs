using System;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowDictionary.Property.Editor;

namespace WindowDictionary.Property.Creator
{
    /// <summary>
    /// Interaction logic for CProperty.xaml
    /// </summary>
    public partial class CProperty : ListViewItem
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the PropertyItem object.
        /// </summary>
        public PropertyItem PropertyItem
        {
            get { return (PropertyItem)GetValue(PropertyItemProperty); }
            set { SetValue(PropertyItemProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for PropertyItem.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PropertyItemProperty =
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(CProperty));

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public CProperty()
        {
            DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Delete_Property_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you wish to delete this property?", "Caution", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                PropertyGroup group = PropertyItem.Parent;
                IPropertyControl parent = group.Parent;
                parent.PropertyGroups.Remove(group);
            }
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box == null)
                return;

            box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if (box == null)
                return;

            Regex expression;
            try
            {
                expression = new Regex(box.Text);
            }
            catch (Exception error)
            {
                MessageBox.Show("Invalid Regular Expression:\n" + error.Message, "Invalid Input", MessageBoxButton.OK);
                return;
            }

            string source = PropertyItem.Regex;
            if ((source != box.Text)
                && (PropertyItem.Values.Count > 0))
            {
                MessageBoxResult result = MessageBox.Show("This new expression could remove property values.\nDo you wish to continue?", "Caution", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    expression = new Regex(box.Text);

                    string[] items = PropertyItem.Values.ToArray();
                    foreach (string item in items)
                    {
                        if (expression.IsMatch(item))
                            continue;

                        PropertyItem.Values.Remove(item);
                    }
                }
                else
                {
                    box.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
                }
            }
        }

        private void Add_PropertyValue_Button_Click(object sender, RoutedEventArgs e)
        {
            Regex expression;
            try
            {
                expression = new Regex(PropertyItem.Regex);
            }
            catch (Exception error)
            {
                MessageBox.Show("Invalid Regular Expression:\n" + error.Message, "Invalid Input", MessageBoxButton.OK);
                value.Focus();
                return;
            }

            if (expression.IsMatch(value.Text))
            {
                PropertyItem.Values.Add(value.Text);
                value.Clear();
                value.Focus();
            }
            else
            {
                MessageBox.Show("The value you have provided does not meet the (regex) regular expression", "Invalid Input", MessageBoxButton.OK);
                value.Focus();
            }
        }

        private void Delete_PropertyValue_Button_Click(object sender, RoutedEventArgs e)
        {
            if (values.SelectedItem == null)
                return;

            PropertyItem.Values.Remove(values.SelectedItem.ToString());
        }

        private void value_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Add_PropertyValue_Button_Click(sender, new RoutedEventArgs());
            }
        }

        private void Add_Dependency_Button_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            if (combo == null)
                return;

            EPropertyType type = (EPropertyType)combo.SelectedIndex;

            MessageBoxResult result = MessageBox.Show("This may remove property values.\nDo you wish to continue?", "Caution", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                switch (type)
                {
                    case EPropertyType.CheckBox:
                        values.IsEnabled = false; 
                        PropertyItem.Values.Clear();
                        PropertyItem.Values.Add("True");
                        PropertyItem.Values.Add("False");
                        values.SelectedItem = null;
                        value.IsEnabled = false;
                        regex.IsEnabled = false;
                        PropertyItem.Regex = "[True|False]";
                        add.IsEnabled = false;
                        delete.IsEnabled = false;
                        break;
                    case EPropertyType.ComboBox:
                        if (PropertyItem.Type.EPropertyType == EPropertyType.CheckBox)
                        {
                            PropertyItem.Values.Clear();
                            PropertyItem.Regex = ".*";
                        }
                        
                        values.IsEnabled = true;
                        value.IsEnabled = true;
                        regex.IsEnabled = true;
                        add.IsEnabled = true;
                        delete.IsEnabled = true;

                        break;
                    case EPropertyType.ComboBoxEdit:
                        if (PropertyItem.Type.EPropertyType == EPropertyType.CheckBox)
                        {
                            PropertyItem.Values.Clear();
                            PropertyItem.Regex = ".*";
                        }

                        values.IsEnabled = true;
                        value.IsEnabled = true;
                        regex.IsEnabled = true;
                        add.IsEnabled = true;
                        delete.IsEnabled = true;

                        break;
                    case EPropertyType.TextBox:
                        if (PropertyItem.Type.EPropertyType == EPropertyType.CheckBox)
                        {
                            PropertyItem.Values.Clear();
                            PropertyItem.Regex = ".*";
                        }

                        values.IsEnabled = true;
                        value.IsEnabled = true;
                        regex.IsEnabled = true;
                        add.IsEnabled = true;
                        delete.IsEnabled = true;

                        break;
                }
            }
        }
    }
}
