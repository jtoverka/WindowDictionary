using System.Windows;
using System.ComponentModel;
using System.Windows.Input;
using System;

namespace WindowDictionary
{
    /// <summary>
    /// Interaction logic for LayerTransparency.xaml
    /// </summary>
    public partial class LayerTransparency : Window, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Move dialog box boolean
        /// </summary>
        private bool mRestoreForDragMove;

        #endregion

        #region Properties

        private System.Windows.Forms.DialogResult _Result;
        /// <summary>
        /// Dialog Box Result
        /// </summary>
        public System.Windows.Forms.DialogResult Result
        {
            get { return this._Result; }
            set
            {
                if (this._Result == value)
                    return;

                this._Result = value;
                OnPropertyChanged("Result");
            }
        }

        private short _Transparency = 0;
        /// <summary>
        /// Layer Transparency
        /// </summary>
        public short Transparency
        {
            get { return _Transparency; }
            set
            {
                if (_Transparency == value)
                    return;

                if (value < 0 || value > 90)
                {
                    _ = MessageBox.Show("Transparency values must be between 0 and 90", "Warning");
                }
                else
                    _Transparency = value;

                OnPropertyChanged("Transparency");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public LayerTransparency()
        {
            Transparency = 0;
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// Constructor with transparency
        /// </summary>
        /// <param name="transparency"></param>
        public LayerTransparency(netDxf.Transparency transparency)
        {
            Transparency = transparency.Value;
            DataContext = this;
            InitializeComponent();
        }

        #endregion

        #region Functions

        /// <summary>
        /// Close window process
        /// </summary>
        private void CloseApplication()
        {
            this.Close();
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Event that is raised when a property is changed on a component
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Method to invoke a property changed event
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Allows user to move the dialog box from the menu bar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                if (this.mRestoreForDragMove)
                {
                    this.mRestoreForDragMove = false;

                    var point = PointToScreen(e.MouseDevice.GetPosition(this));

                    Left = point.X - (RestoreBounds.Width * 0.5);
                    Top = point.Y;

                    WindowState = WindowState.Normal;
                }

                this.DragMove();
            }
        }

        /// <summary>
        /// Window minimize button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Minimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Cancel action on dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.Cancel;
            CloseApplication();
        }

        /// <summary>
        /// Ok action on dialog box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.OK;
            CloseApplication();
        }

        #endregion

        private void Combo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                short value = System.Convert.ToSByte(e.Text);

                if (value < 0 || value > 90)
                {
                    _ = MessageBox.Show("Transparency values must be between 0 and 90", "Warning");
                    e.Handled = true;
                }
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Transparency values must be between 0 and 90", "Warning");
                e.Handled = true;
            }
        }
    }
}
