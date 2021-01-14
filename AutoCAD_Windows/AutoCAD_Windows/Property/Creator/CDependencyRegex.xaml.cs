using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WindowDictionary.Property.Creator
{
    /// <summary>
    /// Interaction logic for CDependencyRegex.xaml
    /// </summary>
    public partial class CDependencyRegex : Window
    {
        #region Fields

        private bool changedSource = false;

        #endregion

        #region Properties
        #region Property - DependencyItem : DependencyItem

        /// <summary>
        /// Gets or Sets the DependencyItem property
        /// </summary>
        public DependencyItem DependencyItem
        {
            get { return (DependencyItem)GetValue(DependencyItemProperty); }
            set { SetValue(DependencyItemProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for DependencyItem.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty DependencyItemProperty =
            DependencyProperty.Register("DependencyItem", typeof(DependencyItem), typeof(CDependencyRegex));


        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// Initialize a new instance of this class
        /// </summary>
        public CDependencyRegex()
        {
            DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Delegates, Events, Handlers

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!changedSource)
                return;

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
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((!(sender is TextBox box))
                || !this.IsLoaded)
                return;

            changedSource = true;

            box.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        }

        #endregion
    }
}
