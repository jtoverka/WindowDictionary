using System;
using System.Collections.Specialized;
using System.Windows.Threading;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WindowDictionary.Resources;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Interaction logic for ListViewSelection.xaml
    /// </summary>
    public partial class ListViewSelection : ListViewItem, INotifyPropertyChanged
    {
        #region Fields

        private DispatcherTimer timer = new DispatcherTimer();

        #endregion

        #region Properties

        private string _Label;
        /// <summary>
        /// The title displayed
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

        private PropertyItem _Item;
        /// <summary>
        /// 
        /// </summary>
        public PropertyItem Item
        {
            get { return _Item; }
            set
            {
                if (_Item == value)
                    return;

                _Item = value;
                value.Values.CollectionChanged += ListItems_CollectionChanged;

                OnPropertyChanged("Item");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public ListViewSelection()
        {
            DataContext = this;

            timer.Interval = TimeSpan.FromSeconds(5);
            timer.Tick += Popup_Timer_Tick;

            InitializeComponent();

            Up_Button.IsEnabled = false;
            Down_Button.IsEnabled = false;
            Top_Button.IsEnabled = false;
            Bottom_Button.IsEnabled = false;
            Remove_Button.IsEnabled = false;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Opens the popup timer
        /// </summary>
        public void TriggerPopup(string error)
        {
            popupText.MaxWidth = text.ActualWidth;
            popup.Width = text.ActualWidth;

            popup.IsOpen = false;
            timer.Stop();

            popupText.Text = error;
            popup.IsOpen = true;
            timer.Start();
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// RoutedEvent Event Triggered from Add Button
        /// </summary>
        public event RoutedEventHandler AddClick;

        /// <summary>
        /// PreviewKeyDown Event triggered from TextBox
        /// </summary>
        public event KeyEventHandler TextBox_PreviewKeyDown;

        /// <summary>
        /// PreviewTextInput Event triggered from TextBox
        /// </summary>
        public event TextCompositionEventHandler TextBox_PreviewTextInput;

        /// <summary>
        /// 
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// PropertyChanged Event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Modify UI depending on the ListItems Collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        /// <summary>
        /// Add text from textbox to ListItems Collection and clear textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            AddClick?.Invoke(this, e);
        }

        /// <summary>
        /// Remove Selected Items from ListItems Collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Remove_Button_Click(sender, e, list.SelectedItems, Item.Values);
        }

        /// <summary>
        /// Move Selected Items Up in ListItems Collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Up_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Up_Button_Click(sender, e, list.SelectedItems, Item.Values);
        }

        /// <summary>
        /// Move Selected Items to the Top of ListItems Collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Top_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Top_Button_Click(sender, e, list.SelectedItems, Item.Values);
        }

        /// <summary>
        /// Move Selected Items Down ListItems Collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Down_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Down_Button_Click(sender, e, list.SelectedItems, Item.Values);
        }

        /// <summary>
        /// Move Selected Items to the Bottom of ListItems Collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bottom_Button_Click(object sender, RoutedEventArgs e)
        {
            UILibrary.Bottom_Button_Click(sender, e, list.SelectedItems, Item.Values);
        }

        /// <summary>
        /// Trigger TextBox_PreviewKeyDown event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox_PreviewKeyDown?.Invoke(this, e);
        }

        /// <summary>
        /// Trigger TextBox_PreviewTextInput event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox_PreviewTextInput?.Invoke(this, e);
        }

        /// <summary>
        /// Close error popup
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Popup_Timer_Tick(object sender, EventArgs e)
        {
            popup.IsOpen = false;
            timer.Stop();
        }

        #endregion
    }
}
