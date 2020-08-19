using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoadLinetype.xaml
    /// </summary>
    public partial class LoadLinetype : Window, INotifyPropertyChanged
    {
        #region Fields

        private bool mRestoreForDragMove;

        #endregion

        #region Properties

        public System.Windows.Forms.DialogResult Result
        {
            get;
            protected set;
        } = System.Windows.Forms.DialogResult.None;

        private netDxf.DxfDocument _document;
        public netDxf.Collections.Linetypes LinetypeCollection
        {
            get { return this._document.Linetypes; }
        }

        private readonly netDxf.DxfDocument _tempDocument = new netDxf.DxfDocument();
        public netDxf.Collections.Linetypes SelectedItems
        {
            get 
            {
                _tempDocument.Linetypes.Clear();
                foreach (object item in Items.SelectedItems)
                {
                    _tempDocument.Linetypes.Add(item as netDxf.Tables.Linetype);
                }

                return _tempDocument.Linetypes;
            }
        }

        #endregion

        #region Constructors

        public LoadLinetype()
        {
            DataContext = this;
            _document = new netDxf.DxfDocument();
            _document.Linetypes.Clear();
            InitializeComponent();
        }

        public LoadLinetype(netDxf.DxfDocument document)
        {
            DataContext = this;
            _document = document;
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.Cancel;
            CloseApplication();
        }

        private void File_Load_Click(object sender, RoutedEventArgs e)
        {
            using var window = new System.Windows.Forms.OpenFileDialog()
            {
                Filter = "Linetype (*.lin)|*.lin",
                Multiselect = false,
                CheckFileExists = true,
                Title = "Select Linetype File",
            };
            window.ShowDialog();

            string filename = window.FileName;
            if (filename != null && filename != "")
            {
                _document.Linetypes.Clear();
                foreach (string linetype in _document.Linetypes.NamesFromFile(filename))
                {
                    try
                    {
                        _document.Linetypes.AddFromFile(filename, linetype, true);
                    }
                    catch (Exception E)
                    {

                    }
                }
                OnPropertyChanged("LinetypeCollection");
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            this.Result = System.Windows.Forms.DialogResult.OK;
            CloseApplication();
        }

        #endregion
    }
}
