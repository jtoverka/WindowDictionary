using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using AutoCAD_Windows.Converters;
// using netDxf;

namespace AutoCAD_Windows
{
    /// <summary>
    /// Interaction logic for Lineweight.xaml
    /// </summary>
    public partial class Lineweight : Window, INotifyPropertyChanged
    {
        #region Fields

        private bool mRestoreForDragMove;

        #endregion

        #region Properties

        private netDxf.Lineweight _InitialLineweight = netDxf.Lineweight.Default;
        public netDxf.Lineweight InitialLineweight
        {
            get { return this._InitialLineweight; }
            protected set
            {
                if (this._InitialLineweight == value)
                    return;

                this._InitialLineweight = value;
                OnPropertyChanged("InitialLineweight");
            }
        }

        private netDxf.Lineweight _SelectedLineweight = netDxf.Lineweight.Default;

        public netDxf.Lineweight SelectedLineweight
        {
            get { return this._SelectedLineweight; }
            set
            {
                if (this._SelectedLineweight == value)
                    return;

                this._SelectedLineweight = value;
                OnPropertyChanged("SelectedLineweight");
            }
        }

        public System.Windows.Forms.DialogResult Result
        {
            get;
            protected set;
        } = System.Windows.Forms.DialogResult.None;

        #endregion

        #region Constructors
        public LineweightMode Mode { get; protected set; }

        public enum LineweightMode
        {
            All,
            Layer,
        }

        public Lineweight()
        {
            DataContext = this;
            Mode = LineweightMode.All;
            this.InitialLineweight = netDxf.Lineweight.Default;
            this.SelectedLineweight = netDxf.Lineweight.Default;
            InitializeComponent();

            var propertypath = new PropertyPath(ListView.SelectedIndexProperty)
            {
                Path = "SelectedLineweight",
            };
            var converter = new LineweightConverter();
            var bind = new Binding()
            {
                Path = propertypath,
                Converter = converter,
                ConverterParameter="1",
            };

            Lineweights.SetBinding(ListView.SelectedIndexProperty, bind);
            
            OnPropertyChanged("SelectedLineweight");
        }

        public Lineweight(netDxf.Lineweight lineweight)
        {
            DataContext = this;
            Mode = LineweightMode.All;
            this.InitialLineweight = lineweight;
            this.SelectedLineweight = lineweight;

            InitializeComponent();

            var propertypath = new PropertyPath(ListView.SelectedIndexProperty)
            {
                Path = "SelectedLineweight",
            };
            var converter = new LineweightConverter();
            var bind = new Binding()
            {
                Path = propertypath,
                Converter = converter,
                ConverterParameter = "1",
            };
            Lineweights.SetBinding(ListView.SelectedIndexProperty, bind);
            
            OnPropertyChanged("SelectedLineweight");
        }
        public Lineweight(netDxf.Lineweight lineweight, LineweightMode mode)
        {
            DataContext = this;

            InitializeComponent();

            var propertypath = new PropertyPath(ListView.SelectedIndexProperty)
            {
                Path = "SelectedLineweight",
            };
            var converter = new LineweightConverter();
            Binding bind;

            if (mode == LineweightMode.Layer)
            {
                Lineweights.Items.Remove(ByBlock);
                Lineweights.Items.Remove(ByLayer);

                bind = new Binding()
                {
                    Path = propertypath,
                    Converter = converter,
                    ConverterParameter="4",
                };
            }
            else
            {
                bind = new Binding()
                {
                    Path = propertypath,
                    Converter = converter,
                    ConverterParameter = "1",
                };
            }

            Lineweights.SetBinding(ListView.SelectedIndexProperty, bind);

            Mode = mode;
            switch (mode)
            {
                case LineweightMode.All:
                    this.InitialLineweight = lineweight;
                    this.SelectedLineweight = lineweight;
                    break;
                case LineweightMode.Layer:

                    if ((this.InitialLineweight == netDxf.Lineweight.ByBlock) || (this.InitialLineweight == netDxf.Lineweight.ByLayer))
                    {
                        this.InitialLineweight = netDxf.Lineweight.Default;
                        this.SelectedLineweight = netDxf.Lineweight.Default;
                    }
                    else
                    {
                        this.InitialLineweight = lineweight;
                        this.SelectedLineweight = lineweight;
                    }
                    break;
            }

            OnPropertyChanged("SelectedLineweight");
        }

        #endregion

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
        private void Window_Exit_Click(object sender, RoutedEventArgs e)
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
    }
}
