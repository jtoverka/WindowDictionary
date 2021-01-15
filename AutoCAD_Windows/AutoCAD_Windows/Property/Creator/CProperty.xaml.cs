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
            if (!(sender is TextBox box)
                || (!this.IsLoaded))
                return;

            if (box.Text == PropertyItem.Parent.Title)
                return;

            if (this.PropertyItem.Parent.Parent.IsPropertyGroupUnique(box.Text))
            {
                box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
            else
            {
                e.Handled = true;
                MessageBox.Show("Group names must be unique", "Warning", MessageBoxButton.OK);
                box.GetBindingExpression(TextBox.TextProperty).UpdateTarget();
            }
        }

        /// <summary>
        /// Checks the validity of the regular expression
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if ((!(sender is TextBox box))
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
            CPropertyRegex window = new CPropertyRegex
            {
                PropertyItem = this.PropertyItem
            };
            window.ShowDialog();
        }

        /// <summary>
        /// Change properties of other controls based on the selection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((!(sender is ComboBox combo))
                || !this.IsLoaded)
                return;

            EPropertyType type = (EPropertyType)combo.SelectedIndex;

            MessageBoxResult result = MessageBox.Show("This may remove property values.\nDo you wish to continue?", "Caution", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                UpdateUI(type);
            }
        }

        private void UpdateUI(EPropertyType type)
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
                    if (PropertyItem?.Type.EPropertyType == EPropertyType.CheckBox)
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
                    if (PropertyItem?.Type.EPropertyType == EPropertyType.CheckBox)
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
                    if (PropertyItem?.Type.EPropertyType == EPropertyType.CheckBox)
                    {
                        PropertyItem.Values.Clear();
                        PropertyItem.Regex = ".*";
                    }

                    if ((PropertyItem?.Type.EPropertyType == EPropertyType.TextBox)
                        && PropertyItem.Values.Count == 0)
                        add.IsEnabled = true;
                    else
                        add.IsEnabled = false;

                    values.IsEnabled = true;
                    value.IsEnabled = true;
                    delete.IsEnabled = true;

                    break;
                default:
                    break;
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

            if ((PropertyItem.Type.EPropertyType == EPropertyType.TextBox)
                && PropertyItem.Values.Count > 0)
                add.IsEnabled = false;
            else
                add.IsEnabled = true;
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

            if ((PropertyItem.Type.EPropertyType == EPropertyType.TextBox)
                && PropertyItem.Values.Count > 0)
                add.IsEnabled = false;
            else
                add.IsEnabled = true;
        }

        /// <summary>
        /// Check if enter was pressed, then add value to values collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Value_PreviewKeyDown(object sender, KeyEventArgs e)
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
            PropertyItem property = new PropertyItem()
            {
                Collapsible = this.PropertyItem.Collapsible,
                CollectionRegex = this.PropertyItem.CollectionRegex,
                Help = this.PropertyItem.Help,
                Parent = this.PropertyItem.Parent,
                Regex = this.PropertyItem.Regex,
                Type = this.PropertyItem.Type,
            };
            foreach (string item in this.PropertyItem.Values)
            {
                property.Values.Add(item);
            }
            foreach (DependencyItem item in this.PropertyItem.DependencyItems)
            {
                property.DependencyItems.Add(item);
            }
            window.PropertyItem = property;
            window.ShowDialog();

            if (window.Result != WindowDictionary.Resources.DialogResult.OK)
                return;

            this.PropertyItem.DependencyItems.Clear();
            foreach (DependencyItem item in window.DependencyItems)
            {
                this.PropertyItem.DependencyItems.Add(item);
            }
            this.PropertyItem.CollectionRegex = window.PropertyItem.CollectionRegex;
            this.PropertyItem.Collapsible = window.PropertyItem.Collapsible;
        }

        private void ListViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            if (PropertyItem == null)
            {
                UpdateUI(EPropertyType.TextBox);
            }
            else
            {
                UpdateUI(PropertyItem.Type.EPropertyType);
            }
        }

        #endregion
    }
}
