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
        #region Properties
        #region Property - Window : Window

        private Window _Window;

        /// <summary>
        /// Gets or Sets the main window
        /// </summary>
        public Window Window
        {
            get { return _Window; }
            set { _Window = value; }
        }

        #endregion
        #region Property - MinVisibility : bool

        /// <summary>
        /// Gets or Sets the minimize button visibility
        /// </summary>
        public Visibility MinVisibility
        {
            get { return (Visibility)GetValue(MinVisibilityProperty); }
            set { SetValue(MinVisibilityProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for MinVisibility.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MinVisibilityProperty =
            DependencyProperty.Register("MinVisibility", typeof(Visibility), typeof(WindowControl));

        #endregion
        #region Property - MaxVisibility : bool

        /// <summary>
        /// Gets or Sets the maximize button visibility
        /// </summary>
        public Visibility MaxVisibility
        {
            get { return (Visibility)GetValue(MaxVisibilityProperty); }
            set { SetValue(MaxVisibilityProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for MaxVisibility.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty MaxVisibilityProperty =
            DependencyProperty.Register("MaxVisibility", typeof(Visibility), typeof(WindowControl));

        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public WindowControl()
        {
            this.DataContext = this;
            this.MinVisibility = Visibility.Visible;
            this.MaxVisibility = Visibility.Visible;
            InitializeComponent();
        }

        #endregion

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
