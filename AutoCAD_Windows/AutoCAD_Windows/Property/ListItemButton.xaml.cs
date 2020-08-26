using System.ComponentModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListItemButton.xaml
    /// </summary>
    public partial class ListItemButton : ListViewItem, INotifyPropertyChanged
    {
        #region Properties

        private PropertyGroup _ParentGroup;
        /// <summary>
        /// 
        /// </summary>
        public PropertyGroup ParentGroup
        {
            get { return this._ParentGroup; }
            set
            {
                if (this._ParentGroup == value)
                    return;

                this._ParentGroup = value;
                OnPropertyChanged("ParentGroup");
            }
        }

        private string _Label;
        /// <summary>
        /// Text displayed 
        /// </summary>
        public string Label
        {
            get { return this._Label; }
            set
            {
                if (this._Label == value)
                    return;

                this._Label = value;
                OnPropertyChanged("Label");
            }
        }

        private string _ButtonText;
        /// <summary>
        /// Text displayed 
        /// </summary>
        public string ButtonText
        {
            get { return this._ButtonText; }
            set
            {
                if (this._ButtonText == value)
                    return;

                this._ButtonText = value;
                OnPropertyChanged("ButtonText");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public ListItemButton()
        {
            DataContext = this;

            InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Invoked on button click event
        /// </summary>
        public event RoutedEventHandler Click;
        
        /// <summary>
        /// Invoked on property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, new RoutedEventArgs());
        }
    }
}
