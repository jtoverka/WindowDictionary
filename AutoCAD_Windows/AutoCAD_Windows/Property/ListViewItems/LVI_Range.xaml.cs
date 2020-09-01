using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
using WindowDictionary.Property.Logic;
using WindowDictionary.Resources;
using System.Windows;
using System.Windows.Data;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for LVI_Range.xaml
    /// </summary>
    public partial class LVI_Range : ListViewItem, INotifyPropertyChanged
    {
        #region Fields

        private Range _Root;
        private Range _SelectedRange;
        readonly DispatcherTimer maxTimer = new DispatcherTimer();
        readonly DispatcherTimer minTimer = new DispatcherTimer();

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Range Root
        {
            get { return this._Root; }
            set
            {
                if (this._Root == value)
                    return;

                this._Root = value;
                
                tree.Items.Clear();
                tree.Items.Add(value);

                OnPropertyChanged("Root");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Range SelectedRange
        {
            get { return this._SelectedRange; }
            set
            {
                if (this._SelectedRange == value)
                    return;

                this._SelectedRange = value;

                OnPropertyChanged("SelectedRange");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Represents a range object
        /// </summary>
        public LVI_Range()
        {
            DataContext = this;

            maxTimer.Interval = TimeSpan.FromSeconds(4);
            maxTimer.Tick += MaxTimer_Tick;
            minTimer.Interval = TimeSpan.FromSeconds(4);
            minTimer.Tick += MinTimer_Tick;

            InitializeComponent();

            Range_CheckBox_Checked(this.checkbox, null);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invokes the property changed event.
        /// </summary>
        /// <param name="property"></param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Trigger popup error on Minimum TextBox
        /// </summary>
        /// <param name="error"></param>
        public void TriggerMinPopup(string error)
        {
            minTimer.Stop();
            minTimer.Start();
            min_popup.IsOpen = false;
            min_popupText.Text = error;
            min_popup.IsOpen = true;
        }

        /// <summary>
        /// Trigger popup error on Maximum TextBox
        /// </summary>
        /// <param name="error"></param>
        public void TriggerMaxPopup(string error)
        {
            maxTimer.Stop();
            maxTimer.Start();
            max_popup.IsOpen = false;
            max_popupText.Text = error;
            max_popup.IsOpen = true;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked when a component property is modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Stop timer for minimum TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinTimer_Tick(object sender, EventArgs e)
        {
            minTimer.Stop();
            min_popup.IsOpen = false;
        }

        /// <summary>
        /// Stop timer for maximum TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MaxTimer_Tick(object sender, EventArgs e)
        {
            maxTimer.Stop();
            max_popup.IsOpen = false;
        }

        private void Range_CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (!(checkbox.IsChecked == true))
            {
                checkbox.IsChecked = true;
                return;
            }

            BindingOperations.ClearBinding(this.combobox, ComboBox.ItemsSourceProperty);
            this.combobox.SetBinding(ComboBox.ItemsSourceProperty,
                new Binding()
                {
                    Source = Enum.GetValues(typeof(LogicalOperator)),
                });
            this.combobox.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateSource();

            BindingOperations.ClearBinding(this.combobox, ComboBox.SelectedIndexProperty);
            this.combobox.SetBinding(ComboBox.SelectedIndexProperty,
                new Binding("SelectedRange")
                {
                    Converter = new Converters.LogicRangeConverter(),
                });
            this.combobox.GetBindingExpression(ComboBox.SelectedIndexProperty).UpdateSource();

            this.max.IsEnabled = false;
            this.min.IsEnabled = false;
            this.button.IsEnabled = true;
        }

        private void Range_CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if (!(checkbox.IsChecked == false))
            {
                checkbox.IsChecked = false;
                return;
            }

            BindingOperations.ClearBinding(this.combobox, ComboBox.ItemsSourceProperty);
            this.combobox.SetBinding(ComboBox.ItemsSourceProperty,
                new Binding()
                {
                    Source = Enum.GetValues(typeof(RangeOperator)),
                });
            this.combobox.GetBindingExpression(ComboBox.ItemsSourceProperty).UpdateSource();

            BindingOperations.ClearBinding(this.combobox, ComboBox.SelectedIndexProperty);
            this.combobox.SetBinding(ComboBox.SelectedIndexProperty,
                new Binding("SelectedRange")
                {
                    Converter = new Converters.RangeConverter(),
                });
            this.combobox.GetBindingExpression(ComboBox.SelectedIndexProperty).UpdateSource();

            this.max.IsEnabled = true;
            this.min.IsEnabled = true;
            this.button.IsEnabled = false;
        }

        private void Range_Button_Click(object sender, RoutedEventArgs e)
        {
            var gate = this.SelectedRange as LogicalGate;
            gate.RangeCollection.Add(new LogicalGate());
            
        }

        private void tree_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedRange = tree.SelectedItem as Range;
        }

        #endregion

        private void combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        } vgu       
    }
}
