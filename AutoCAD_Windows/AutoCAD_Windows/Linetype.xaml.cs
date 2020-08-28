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

namespace WindowDictionary
{
    /// <summary>
    /// Interaction logic for Linetype.xaml
    /// </summary>
    public partial class Linetype : Window, INotifyPropertyChanged
    {
        #region Fields

        private bool mRestoreForDragMove;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.DialogResult Result
        {
            get;
            protected set;
        } = System.Windows.Forms.DialogResult.None;


        private netDxf.DxfDocument _document;
        /// <summary>
        /// 
        /// </summary>
        public netDxf.Collections.Linetypes LinetypeCollection
        {
            get { return this._document.Linetypes; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// 
        /// </summary>
        public Linetype()
        {
            DataContext = this;
            _document = new netDxf.DxfDocument();
            _document.Linetypes.Clear();
            InitializeComponent();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="document"></param>
        public Linetype(netDxf.DxfDocument document)
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

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
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

        private void LoadLinetype_Click(object sender, RoutedEventArgs e)
        {
            var window = new LoadLinetype();
            window.ShowDialog();

            if (window.Result == System.Windows.Forms.DialogResult.OK)
            {
                foreach (netDxf.Tables.Linetype item in window.SelectedItems)
                {
                    this._document.Linetypes.Add(item);
                }
            }
        }

        #endregion


    }
}
