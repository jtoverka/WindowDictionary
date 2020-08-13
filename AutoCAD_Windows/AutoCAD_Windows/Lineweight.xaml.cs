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

namespace AutoCAD_Windows
{
    /// <summary>
    /// Interaction logic for Lineweight.xaml
    /// </summary>
    public partial class Lineweight : Window, INotifyPropertyChanged
    {
        #region Constructors

        public Lineweight(int weight)
        {
            DataContext = this;

            if (weight > 0 && weight < weights.Count)
            {
                _OldValue = weight;
                _NewValue = weight;
            }

            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AutoCAD_Windows;component/Lineweight.xaml", System.UriKind.Relative);

            System.Windows.Application.LoadComponent(this, resourceLocater);
        }

        public Lineweight()
        {
            DataContext = this;

            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/AutoCAD_Windows;component/Lineweight.xaml", System.UriKind.Relative);

            System.Windows.Application.LoadComponent(this, resourceLocater);
        }

        #endregion

        #region Properties

        public bool OKCancel { get; set; }

        private int _OldValue = 0;
        public string OldValue
        {
            get { return weights[_OldValue]; }
        }

        private int _NewValue = 0;
        public string NewValue
        {
            get { return weights[_NewValue]; }
        }

        public int SelectedIndex
        {
            get { return _NewValue; }
            set
            {
                if (_NewValue == value)
                    return;

                _NewValue = value;
                OnPropertyChanged("SelectedIndex");
                OnPropertyChanged("NewValue");
            }
        }

        #endregion

        #region fields

        private bool mRestoreForDragMove;

        private readonly List<string> weights = new List<string>()
        {
            "Default",
            "0.00 mm",
            "0.05 mm",
            "0.09 mm",
            "0.13 mm",
            "0.15 mm",
            "0.18 mm",
            "0.20 mm",
            "0.25 mm",
            "0.30 mm",
            "0.35 mm",
            "0.40 mm",
            "0.50 mm",
            "0.53 mm",
            "0.60 mm",
            "0.70 mm",
            "0.80 mm",
            "0.90 mm",
            "1.00 mm",
            "1.06 mm",
            "1.20 mm",
            "1.40 mm",
            "1.58 mm",
            "2.00 mm",
            "2.11 mm",
        };

        #endregion

        #region Delegates, Events, Handlers


        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region Functions

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
            CloseApplication();
        }
        private void CloseApplication()
        {
            this.Close();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            OKCancel = true;
            this.Close();
        }

        #endregion

    }
}
