using System;
using System.Linq;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowDictionary.Property.Editor;
using System.Collections.ObjectModel;

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

        /// <summary>
        /// Delete property button click. Delete this property from parent collection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_Property_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            MessageBoxResult result = MessageBox.Show("Do you wish to delete this property?", "Caution", MessageBoxButton.YesNoCancel);

            if (result == MessageBoxResult.Yes)
            {
                PropertyGroup group = PropertyItem.Parent;
                IPropertyControl parent = group.Parent;
                parent.PropertyGroups.Remove(group);
            }
        }
        
        /// <summary>
        /// Update the title property from PropertyGroup object
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            if ((box == null)
                || !this.IsLoaded)
                return;

            box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        /// <summary>
        /// Checks the validity of the regular expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox box = sender as TextBox;
            if ((box == null)
                || !this.IsLoaded)
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

        /// <summary>
        /// Opens the regex property editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Regex_Edit_Button_Click(object sender, RoutedEventArgs e)
        {
            CPropertyRegex window = new CPropertyRegex();
            window.PropertyItem = this.PropertyItem;
            window.ShowDialog();
        }

        /// <summary>
        /// Change properties of other controls based on the selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            if ((combo == null)
                || !this.IsLoaded)
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
                        add.IsEnabled = true;
                        delete.IsEnabled = true;

                        break;
                }
            }
        }

        /// <summary>
        /// Add value to values collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_PropertyValue_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

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

        /// <summary>
        /// Delete value from values collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Delete_PropertyValue_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((values.SelectedItem == null)
                || !this.IsLoaded)
                return;

            PropertyItem.Values.Remove(values.SelectedItem.ToString());
        }

        /// <summary>
        /// Check if enter was pressed, then add value to values collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void value_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                Add_PropertyValue_Button_Click(sender, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// Opens the dependency editor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Dependency_Button_Click(object sender, RoutedEventArgs e)
        {
            if (!this.IsLoaded)
                return;

            CPropertyDependency window = new CPropertyDependency();
            window.PropertyItem.Collapsible = this.PropertyItem.Collapsible;
            window.PropertyItem.CollectionRegex = this.PropertyItem.CollectionRegex;
            window.PropertyItem.Help = this.PropertyItem.Help;
            window.PropertyItem.Parent = this.PropertyItem.Parent;
            window.PropertyItem.Regex = this.PropertyItem.Regex;
            window.PropertyItem.Type = this.PropertyItem.Type;
            window.PropertyItem.Values.Concat(this.PropertyItem.Values);
            window.PropertyItem.DependencyItems.Concat(this.PropertyItem.DependencyItems);
            window.UpdateList();
            window.ShowDialog();

            if (window.Result != WindowDictionary.Resources.DialogResult.OK)
                return;

            this.PropertyItem.DependencyItems.Clear();
            this.PropertyItem.DependencyItems.Concat(window.DependencyItems);
            this.PropertyItem.CollectionRegex = window.PropertyItem.CollectionRegex;
            this.PropertyItem.Collapsible = window.PropertyItem.Collapsible;
        }

        #endregion
    }
}
