using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WindowDictionary.Extensions;

namespace WindowDictionary
{

    /// <summary>
    /// Interaction logic for Layer.xaml
    /// </summary>
    public partial class Layer : Window, INotifyPropertyChanged
    {
        #region Fields

        private bool mRestoreForDragMove;
        private readonly char[] invalidCharacters = { '\\', '/', ':', '*', '?', '"', '<', '>', '|', ';', ',', '=', '`' };

        #endregion

        #region Properties

        /// <summary>
        /// Stores collection of netDxf.Tables.Layer objects extended to implement INotifyPropertyChanged.
        /// </summary>
        public ObservableCollection<DxfLayerExtended> LayerCollection { get; }

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

        #endregion
        
        #region Constructors

        /// <summary>
        /// Default layer constructor
        /// </summary>
        public Layer()
        {
            LayerCollection = new ObservableCollection<DxfLayerExtended>
            {
                new DxfLayerExtended("0"),
            };
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// Clone layer collection in document for modification
        /// </summary>
        /// <param name="dxfDocument"></param>
        public Layer(netDxf.DxfDocument dxfDocument)
        {
            foreach (netDxf.Tables.Layer layer in dxfDocument.Layers)
            {
                var newLayer = new DxfLayerExtended(layer.Name)
                {
                    IsVisible = layer.IsVisible,
                    IsFrozen = layer.IsFrozen,
                    IsLocked = layer.IsLocked,
                    Plot = layer.Plot,
                    Color = layer.Color,
                    Linetype = layer.Linetype,
                    Lineweight = layer.Lineweight,
                    Transparency = layer.Transparency,
                };
                LayerCollection.Add(newLayer);
            }
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region Function

        private void CloseApplication()
        {
            this.Close();
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Property Changed Event
        /// Updates all bindings when property changes
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes property changed event
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
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

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var image = sender as Image;

            if (e.LeftButton != MouseButtonState.Pressed || image == null)
                return;

            var mainLayer = image.DataContext as DxfLayerExtended;
            var tag = image.Tag as string;


            if (!LayerList.SelectedItems.Contains(mainLayer))
                LayerList.SelectedItems.Clear();

            if (LayerList.SelectedItems.Count == 0)
                LayerList.SelectedItems.Add(mainLayer);

            foreach (DxfLayerExtended layer in LayerList.SelectedItems)
            {
                if (layer != mainLayer)
                {
                    switch (tag)
                    {
                        case "IsVisible":
                            layer.IsVisible = !mainLayer.IsVisible;
                            break;
                        case "IsFrozen":
                            layer.IsFrozen = !mainLayer.IsFrozen;
                            break;
                        case "IsLocked":
                            layer.IsLocked = !mainLayer.IsLocked;
                            break;
                        case "Plot":
                            layer.Plot = !mainLayer.Plot;
                            break;
                    }
                    layer.OnPropertyChanged(tag);
                }
            }

            switch (tag)
            {
                case "IsVisible":
                    mainLayer.IsVisible = !mainLayer.IsVisible;
                    break;
                case "IsFrozen":
                    mainLayer.IsFrozen = !mainLayer.IsFrozen;
                    break;
                case "IsLocked":
                    mainLayer.IsLocked = !mainLayer.IsLocked;
                    break;
                case "Plot":
                    mainLayer.Plot = !mainLayer.Plot;
                    break;
            }
            mainLayer.OnPropertyChanged(tag);

            e.Handled = true;
        }

        private void New_Layer_Click(object sender, RoutedEventArgs e)
        {
            string name = null;
            bool found;
            for (int i = 0; (i < LayerCollection.Count + 1) && (name == null); i++)
            {
                found = false;
                for (int j = 0; (j < LayerCollection.Count) && !found; j++)
                {
                    if (LayerCollection[j].Name == "Layer" + (i + 1))
                        found = true;
                }
                if (!found)
                    name = "Layer" + (i + 1);
            }
            if (name != null)
                LayerCollection.Add(new DxfLayerExtended(name));
        }

        private void Delete_Layer_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<DxfLayerExtended> layers = new ObservableCollection<DxfLayerExtended>(LayerList.SelectedItems.OfType<DxfLayerExtended>());

            int index = LayerList.SelectedIndex;

            for (int i = 0; i < layers.Count; i++)
            {
                DxfLayerExtended layer = layers[i];

                if (layer.Name != "0")
                {
                    LayerList.SelectedItems.Remove(layer);
                    LayerCollection.Remove(layer);
                }
                else
                    MessageBox.Show("Cannot Delete Default Layer \"0\"", "Warning");
            }

            if (LayerList.Items.Count > index)
                LayerList.SelectedIndex = index;
            else
                LayerList.SelectedIndex = LayerList.Items.Count - 1;
        }

        /// <summary>
        /// Checks if a string is valid as a table object name.
        /// </summary>
        /// <param name="name">String to check.</param>
        /// <returns>True if the string is valid as a table object name, or false otherwise.</returns>
        public bool IsValidName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return false;

            return name.IndexOfAny(invalidCharacters) == -1;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textbox = sender as TextBox;
            var layer = textbox.DataContext as DxfLayerExtended;

            if (layer.Name == "0" || !IsValidName(e.Text))
            {
                e.Handled = true;
            }
        }

        private void Lineweight_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var layer = ((TextBlock)sender).DataContext as DxfLayerExtended;
            var window = new WindowDictionary.Lineweight(layer.Lineweight, Lineweight.LineweightMode.Layer);

            window.ShowDialog();

            layer.Lineweight = window.SelectedLineweight;
            layer.OnPropertyChanged("Lineweight");
        }

        private void LayerTransparency_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
                return;

            var layer = ((TextBlock)sender).DataContext as DxfLayerExtended;
            var window = new WindowDictionary.LayerTransparency(layer.Transparency);

            window.ShowDialog();

            if (window.Result == System.Windows.Forms.DialogResult.OK)
            {
                layer.Transparency = new netDxf.Transparency(window.Transparency);
                layer.OnPropertyChanged("Transparency");
            }
        }

        #endregion
    }
}