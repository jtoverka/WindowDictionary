﻿using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
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

        public Lineweight()
        {
            DataContext = this;
            InitializeComponent();
        }

        public Lineweight(netDxf.Lineweight lineweight)
        {
            DataContext = this;
            this.InitialLineweight = lineweight;
            this.SelectedLineweight = lineweight;
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
