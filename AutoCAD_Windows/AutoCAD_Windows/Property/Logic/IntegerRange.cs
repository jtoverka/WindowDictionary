using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property.Logic
{
    /// <summary>
    /// Represents an integer range of two integers.
    /// </summary>
    public class IntegerRange : Range, IEquatable<IntegerRange>, INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// Gets or Sets the Parent Object
        /// </summary>
        [XmlIgnore]
        public override object Parent { get; set; }

        /// <summary>
        /// Gets or Sets the label
        /// </summary>
        public override string Label
        {
            get { return "Integer: { " + Min.ToString() + " - " + Max.ToString() + " }"; }
        }

        private int _Min;
        /// <summary>
        /// Gets or Sets the Min Component.
        /// </summary>
        [XmlElement("Min")]
        public override object Min
        {
            get { return this._Min; }
            set
            {
                int value2 = Convert.ToInt32(value);

                if (value2 == this._Min)
                    return;

                this._Min = value2;

                OnPropertyChanged("Min");
                OnPropertyChanged("Label");
            }
        }

        private int _Max;
        /// <summary>
        /// Gets or Sets the Max Component.
        /// </summary>
        [XmlElement("Max")]
        public override object Max
        {
            get { return this._Max; }
            set
            {
                int value2 = Convert.ToInt32(value);

                if (value2 == this._Max)
                    return;

                this._Max = value2;

                OnPropertyChanged("Min");
                OnPropertyChanged("Label");
            }
        }

        /// <summary>
        /// Collection of <see cref="Range"/> objects.
        /// </summary>
        [XmlElement("RangeCollection")]
        public override ObservableCollection<Range> RangeCollection { get; } = new ObservableCollection<Range>();

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of IntegerRange.
        /// </summary>
        public IntegerRange()
        {
            this.Min = int.MinValue;
            this.Max = int.MaxValue;
            RangeCollection.CollectionChanged += CollectionChanged;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Check if the components of two IntegerRanges are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IntegerRange other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Check if the comopnents of two IntegerRanges are equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(IntegerRange a, IntegerRange b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        /// <summary>
        /// Check if an integer is valid with the given range.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(IntegerRange a, int value)
        {
            return a.IsValid(value);
        }

        /// <summary>
        /// Check if an integer is valid with this range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            try
            {
                value = System.Convert.ToInt32(value);
            }
            catch { }

            if (value.GetType() != typeof(int))
                throw new ArgumentException("The value needs to be integer");

            var convert = Convert.ToInt32(value);

            if (convert < this._Min)
                return false;

            if (convert > this._Max)
                return false;

            return true;
        }

        /// <summary>
        /// Obtains a string that represents the IntegerRange.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}{2}{1}", this._Min, this._Max, Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Obtains a string that represents the IntegerRange.
        /// </summary>
        /// <param name="provider">An IFormatProvider interface implementation that supplies culture-specific formatting information. </param>
        /// <returns>A string text.</returns>
        public string ToString(IFormatProvider provider)
        {
            return string.Format("{0}{2} {1}", this._Min.ToString(provider), this._Max.ToString(provider), Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Event property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke Propaerty Changed event
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems == null)
                return;

            foreach (Range item in e.NewItems)
            {
                item.Parent = this;
            }
        }

        #endregion
    }
}
