using System;
using System.Windows;
using System.Windows.Controls;

namespace WindowDictionary.Resources
{
    /// <summary>
    /// Interaction logic for WindowControl.xaml
    /// </summary>
    public partial class WindowControl : UserControl
    {
        private Window _Window;

        /// <summary>
        /// Gets or Sets the main window
        /// </summary>
        public Window Window
        {
            get { return _Window; }
            set { _Window = value; }
        }

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public WindowControl()
        {
            InitializeComponent();
        }

        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked when the Window_Exit_Button is clicked
        /// </summary>
        public event EventHandler WindowExit;

        /// <summary>
        /// Window minimize button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Minimize_Click(object sender, RoutedEventArgs e)
        {
            _Window.WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Window resize button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Resize_Click(object sender, RoutedEventArgs e)
        {
            if (_Window.WindowState == WindowState.Maximized)
                _Window.WindowState = WindowState.Normal;
            else
                _Window.WindowState = WindowState.Maximized;
        }

        /// <summary>
        /// Delegate the cancel button to the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Exit_Click(object sender, RoutedEventArgs e)
        {
            WindowExit.Invoke(this, new EventArgs());
        }

        #endregion
    }
}
