using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Threading;
using WindowDictionary.Property.Logic;
using WindowDictionary.Resources;
using System.Windows;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace WindowDictionary.Property.ListViewItems
{
    /// <summary>
    /// Interaction logic for LVI_Range.xaml
    /// </summary>
    public partial class LVI_Range : ListViewItem, INotifyPropertyChanged
    {
        #region Fields

        private bool updating = false;
        private LVI_RangeItem RangeItem;
        private Range _SelectedRange;

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<Range> Root { get; } = new ObservableCollection<Range>();

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

            InitializeComponent();

            Root.CollectionChanged += Root_CollectionChanged;

            UpdateView();
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
        /// Update View
        /// </summary>
        private void UpdateView()
        {
            if (updating)
                return;
            else
                updating = true;

            if (SelectedRange == null)
            {
                if (Root.Count == 0)
                {
                    updating = false;
                    return;
                }

                SelectedRange = Root[0];
            }

            RangeItem = new LVI_RangeItem();

            RangeItem.CheckBox.IsChecked = (SelectedRange.GetType() == typeof(LogicalGate));

            if (RangeItem.CheckBox.IsChecked.Value)
            {
                RangeItem.ComboBox.SetBinding(ComboBox.ItemsSourceProperty,
                    new Binding()
                    {
                        Source = Enum.GetValues(typeof(LogicalOperator)),
                    });

                RangeItem.ComboBox.SelectedIndex = (int)(SelectedRange as LogicalGate).Operator;
                RangeItem.Max.IsEnabled = false;
                RangeItem.Min.IsEnabled = false;
                RangeItem.Button.IsEnabled = true;
            }
            else
            {
                RangeItem.ComboBox.SetBinding(ComboBox.ItemsSourceProperty,
                    new Binding()
                    {
                        Source = Enum.GetValues(typeof(RangeOperator)),
                    });

                if (SelectedRange.GetType() == typeof(CharRange))
                {
                    RangeItem.ComboBox.SelectedIndex = 0;

                    RangeItem.Min.PreviewTextInput += TextBox_Char_PreviewTextInput;
                    RangeItem.Min.PreviewKeyDown += TextBox_Char_PreviewKeyDown;
                    RangeItem.Max.PreviewTextInput += TextBox_Char_PreviewTextInput;
                    RangeItem.Max.PreviewKeyDown += TextBox_Char_PreviewKeyDown;
                }
                else
                if (SelectedRange.GetType() == typeof(DoubleRange))
                {
                    RangeItem.ComboBox.SelectedIndex = 1;

                    RangeItem.Min.PreviewTextInput += TextBox_Double_PreviewTextInput;
                    RangeItem.Min.PreviewKeyDown += TextBox_Double_PreviewKeyDown;
                    RangeItem.Max.PreviewTextInput += TextBox_Double_PreviewTextInput;
                    RangeItem.Max.PreviewKeyDown += TextBox_Double_PreviewKeyDown;
                }
                else
                if (SelectedRange.GetType() == typeof(IntegerRange))
                {
                    RangeItem.ComboBox.SelectedIndex = 2;

                    RangeItem.Min.PreviewTextInput += TextBox_Integer_PreviewTextInput;
                    RangeItem.Min.PreviewKeyDown += TextBox_Integer_PreviewKeyDown;
                    RangeItem.Max.PreviewTextInput += TextBox_Integer_PreviewTextInput;
                    RangeItem.Max.PreviewKeyDown += TextBox_Integer_PreviewKeyDown;
                }
                else
                    RangeItem.ComboBox.SelectedIndex = 0;

                RangeItem.Max.IsEnabled = true;
                RangeItem.Min.IsEnabled = true;
                RangeItem.Button.IsEnabled = false;
            }

            RangeItem.Min.SetBinding(TextBox.TextProperty, 
                new Binding("Min")
                {
                    Source = SelectedRange,
                });
            RangeItem.Max.SetBinding(TextBox.TextProperty,
                new Binding("Max")
                {
                    Source = SelectedRange,
                });

            RangeItem.Delete.IsEnabled = !(SelectedRange.Parent == this);

            RangeItem.ComboBox.SelectionChanged += Combobox_SelectionChanged;
            RangeItem.CheckBox.Checked += CheckBox_Checked;
            RangeItem.CheckBox.Unchecked += CheckBox_Unchecked;
            RangeItem.Button.Click += Range_Add_Click;
            RangeItem.Delete.Click += Range_Delete_Click;
            listview.Items.Clear();
            listview.Items.Add(RangeItem);

            updating = false;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked when a component property is modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Update view on checkbox unchecked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (updating)
                return;

            Range range = new CharRange();
            int index;

            if (SelectedRange.Parent == this)
            {
                index = Root.IndexOf(SelectedRange);
                Root[index] = range;
            }
            else
            {
                Range item = SelectedRange.Parent as Range;
                index = item.RangeCollection.IndexOf(SelectedRange);
                item.RangeCollection[index] = range;
            }

            SelectedRange = range;

            UpdateView();
        }

        /// <summary>
        /// Update view on checkbox checked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (updating)
                return;

            Range range = new LogicalGate();
            int index;

            if (SelectedRange.Parent == this)
            {
                index = Root.IndexOf(SelectedRange);
                Root[index] = range;
            }
            else
            {
                Range item = SelectedRange.Parent as Range;
                index = item.RangeCollection.IndexOf(SelectedRange);
                item.RangeCollection[index] = range;
            }

            SelectedRange = range;

            UpdateView();
        }

        /// <summary>
        /// Update Range
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var combo = sender as ComboBox;

            if (SelectedRange == null)
            {
                if (Root.Count == 0)
                {
                    updating = false;
                    return;
                }

                SelectedRange = Root[0];
            }

            if (RangeItem.CheckBox.IsChecked.Value)
            {
                LogicalGate gate = SelectedRange as LogicalGate;

                gate.Operator = (LogicalOperator)combo.SelectedIndex;
            }
            else
            {
                Range range = (combo.SelectedIndex) switch
                {
                    0 => new CharRange(),
                    1 => new DoubleRange(),
                    2 => new IntegerRange(),
                    _ => new CharRange(),
                };

                if (SelectedRange.Parent == this)
                {
                    Root.Remove(SelectedRange);
                    Root.Add(range);
                }
                else
                {
                    Range item = SelectedRange.Parent as Range;
                    item.RangeCollection.Remove(SelectedRange);
                    item.RangeCollection.Add(range);
                }

                SelectedRange = range;
            }
        }

        /// <summary>
        /// Add New Range Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Range_Add_Click(object sender, RoutedEventArgs e)
        {
            this.SelectedRange.RangeCollection.Add(new LogicalGate());
        }

        /// <summary>
        /// Delete Range Button Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Range_Delete_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedRange.Parent == this)
            {
                Root.Remove(SelectedRange);
            }
            else
            {
                Range parent = SelectedRange.Parent as Range;
                parent.RangeCollection.Remove(SelectedRange);
            }
        }

        /// <summary>
        /// Update the parent object for each item added to the collection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Root_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
                return;

            foreach (Range item in e.NewItems)
            {
                item.Parent = this;
            }
        }

        private void Tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedRange = tree.SelectedItem as Range;

            UpdateView();
        }

        private void TextBox_Double_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewKeyDown(sender, e);
        }

        private void TextBox_Double_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_Double_PreviewTextInput(sender, e);

            if (e.Handled == true)
                return;

            TextBox textbox = sender as TextBox;
            ListViewItem listviewitem = textbox.DataContext as ListViewItem;
            PropertyItem Item = listviewitem.Tag as PropertyItem;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            if (!Item.ValueRange.IsValid(text))
            {
                e.Handled = true;

            }
        }

        private void TextBox_Integer_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            UILibrary.TextBox_Integer_PreviewKeyDown(sender, e);
        }

        private void TextBox_Integer_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            UILibrary.TextBox_String_PreviewTextInput(sender, e);

            if (e.Handled == true)
                return;

            TextBox textbox = sender as TextBox;
            ListViewItem listviewitem = textbox.DataContext as ListViewItem;
            PropertyItem Item = listviewitem.Tag as PropertyItem;

            string text = UILibrary.TextBox_PreviewTextInput(sender, e);

            if (!Item.ValueRange.IsValid(text))
            {
                e.Handled = true;

            }
        }

        private void TextBox_Char_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                e.Handled = true;
        }

        private void TextBox_Char_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            string text = UILibrary.TextBox_PreviewTextInput(sender, e);
            if (text.Length > 1)
            {
                e.Handled = true;
                RangeItem.TriggerMaxPopup("Only one character allowed!");
            }
        }

        #endregion
    }
}
