using System;
using System.ComponentModel;
using System.Collections.Generic;
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
using System.Drawing;
using System.Drawing.Text;
using System.Collections.ObjectModel;
using FF = System.Drawing.FontFamily;
using OC_FF = System.Collections.ObjectModel.ObservableCollection<System.Drawing.FontFamily>;
namespace AutoCAD_Windows
{
    /// <summary>
    /// Interaction logic for Lineweight.xaml
    /// </summary>
    public partial class Textstyle : Window, INotifyPropertyChanged
    {
        #region Constructors
        
        public Textstyle()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        #region Properties

        private System.Windows.Forms.DialogResult _Result;
        public System.Windows.Forms.DialogResult Result
        {
            get { return this._Result; }
            protected set
            {
                if (this._Result == value)
                    return;

                this._Result = value;
                OnPropertyChanged("Result");
            }
        }

        public OC_FF FontCollection { get; } = new OC_FF(FF.Families.ToList<FF>());

        private FF _SelectedFont;
        public FF SelectedFont
        {
            get { return this._SelectedFont; }
            set
            {
                if (this._SelectedFont == value)
                    return;

                this._SelectedFont = value;
                OnPropertyChanged("SelectedFont");
            }
        }


        private string _FontBox1;
        public string FontBox1
        {
            get { return this._FontBox1; }
            set
            {
                if (this._FontBox1 == value)
                    return;

                this._FontBox1 = value;
                OnPropertyChanged("FontBox1");
            }
        }


        private string _SizeName;
        public string SizeName
        {
            get { return this._SizeName; }
            set
            {
                if (this._SizeName == value)
                    return;

                this._SizeName = value;
                OnPropertyChanged("SizeName");
            }
        }

        #endregion

        #region fields

        private bool mRestoreForDragMove;
        
        #endregion

        #region Delegates, Events, Handlers

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
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
            Result = System.Windows.Forms.DialogResult.Cancel;
            CloseApplication();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = System.Windows.Forms.DialogResult.OK;
            CloseApplication();
        }

        #endregion

        #region Functions

        private void CloseApplication()
        {
            this.Close();
        }

        #endregion
    }
}
