using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace AutoCAD_Windows
{
    /// <summary>
    /// Interaction logic for Layer.xaml
    /// </summary>
    public partial class Layer : Window, INotifyPropertyChanged
    {
        private bool mRestoreForDragMove;

        public ObservableCollection<netDxf.Tables.Layer> LayerCollection { get; }

        private System.Windows.Forms.DialogResult _Result;
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

        public Layer()
        {
            LayerCollection = new ObservableCollection<netDxf.Tables.Layer>
            {
                new netDxf.Tables.Layer("0")
            };
            DataContext = this;
            InitializeComponent();
        }
        #region Function

        private void CloseApplication()
        {
            this.Close();
        }

        #endregion

        #region Delegates, Events, Handlers

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        public void OnPropertyChanged(object sender, string property)
        {
            PropertyChanged?.Invoke(sender, new PropertyChangedEventArgs(property));
        }
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
        /// Window resize button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Resize_Click(object sender, RoutedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Maximized:
                    WindowState = WindowState.Normal;
                    this.Window_Resize_Button.ToolTip = "Maximize";
                    this.imageResizeApp.Source = new BitmapImage(new Uri(@"Application/maximizeAppIcon.ico", UriKind.Relative));
                    this.BorderThickness = new Thickness(0);
                    break;
                case WindowState.Normal:
                    WindowState = WindowState.Maximized;
                    this.Window_Resize_Button.ToolTip = "Restore Down";
                    this.imageResizeApp.Source = new BitmapImage(new Uri(@"Application/toNormalAppIcon.ico", UriKind.Relative));
                    this.BorderThickness = new Thickness(7);
                    break;
            }
        }

        /// <summary>
        /// Change Window State Icon when maximized or minimized
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            switch (WindowState)
            {
                case WindowState.Normal:
                    {
                        this.Window_Resize_Button.ToolTip = "Maximize";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"Application/maximizeAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(0);
                        this.mRestoreForDragMove = false;
                        break;
                    }
                case WindowState.Maximized:
                    {
                        this.Window_Resize_Button.ToolTip = "Restore Down";
                        this.imageResizeApp.Source = new BitmapImage(new Uri(@"Application/toNormalAppIcon.ico", UriKind.Relative));
                        this.BorderThickness = new Thickness(7);
                        this.mRestoreForDragMove = true;
                        break;
                    }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.Cancel;
            CloseApplication();
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.OK;
            CloseApplication();
        }

        #endregion

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;

            if (e.LeftButton != MouseButtonState.Pressed || image == null)
                return;

            var tag = image.Tag as string;
            var layer = image.DataContext as netDxf.Tables.Layer;

            switch (tag)
            {
                case "IsVisible":
                    layer.IsVisible = !layer.IsVisible;
                    image.GetBindingExpression(Image.SourceProperty).UpdateTarget();
                    break;
                case "IsFrozen":
                    layer.IsFrozen = !layer.IsFrozen;
                    image.GetBindingExpression(Image.SourceProperty).UpdateTarget();
                    break;
                case "IsLocked":
                    layer.IsLocked = !layer.IsLocked;
                    image.GetBindingExpression(Image.SourceProperty).UpdateTarget();
                    break;
                case "Plot":
                    layer.Plot = !layer.Plot;
                    image.GetBindingExpression(Image.SourceProperty).UpdateTarget();
                    break;
            }
        }

        private void New_Layer_Click(object sender, RoutedEventArgs e)
        {
            LayerCollection.Add(new netDxf.Tables.Layer("Layer"));
        }
    }
}
