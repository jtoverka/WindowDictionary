using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowDictionary.Property.Creator
{
    /// <summary>
    /// Interaction logic for CPropertyRegex.xaml
    /// </summary>
    public partial class CPropertyRegex : Window
    {
        #region Fields

        bool changedSource = false;

        #endregion

        #region Properties
        #region Property - PropertyItem : PropertyItem

        /// <summary>
        /// Gets or Sets the <see cref="PropertyItem"/> object.
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
            DependencyProperty.Register("PropertyItem", typeof(PropertyItem), typeof(CPropertyRegex));

        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public CPropertyRegex()
        {
            DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Delegates, Events, Handlers

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox box = sender as TextBox;
            if ((box == null)
                || !this.IsLoaded)
                return;

            changedSource = true;

            box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Regex expression;
            try
            {
                expression = new Regex(regex.Text);
            }
            catch (Exception error)
            {
                MessageBox.Show("Invalid Regular Expression:\n" + error.Message, "Invalid Input", MessageBoxButton.OK);
                e.Cancel = true;
                return;
            }

            if (this.changedSource
                && PropertyItem.Values.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("This new expression could remove property values.\nDo you wish to continue?", "Caution", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    expression = new Regex(regex.Text);

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
                    e.Cancel = true;
                }
            }
        }

        #endregion
    }
}
