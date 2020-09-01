using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property.Logic
{
    /// <summary>
    /// Represents a double range of two doubles.
    /// </summary>
    [Serializable]
    public class DoubleRange : Range, IEquatable<DoubleRange>, INotifyPropertyChanged
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
            get { return "Double: { " + Convert.ToDouble(Min).ToString("0.000E0") + " - " + Convert.ToDouble(Max).ToString("0.000E0") + " }"; }
        }

        private double _Min;
        /// <summary>
        /// Gets or Sets the Min Component.
        /// </summary>
        [XmlElement("Min")]
        public override object Min 
        {
            get { return this._Min; }
            set 
            {
                try
                {
                    Double value2 = Convert.ToDouble(value);
                    
                    if (value2 == this._Min)
                        return;

                    this._Min = value2;

                    OnPropertyChanged("Min");
                    OnPropertyChanged("Label");
                }
                catch 
                {
                    this._Min = Double.MinValue;

                    OnPropertyChanged("Min");
                    OnPropertyChanged("Label");
                }
            }
        }

        private double _Max;
        /// <summary>
        /// Gets or Sets the Max Component.
        /// </summary>
        [XmlElement("Max")]
        public override object Max
        {
            get { return this._Max; }
            set
            {
                try
                {
                    Double value2 = Convert.ToDouble(value);

                    if (value2 == this._Max)
                        return;

                    this._Max = value2;

                    OnPropertyChanged("Max");
                    OnPropertyChanged("Label");
                }
                catch
                {
                    this._Max = Double.MinValue;

                    OnPropertyChanged("Max");
                    OnPropertyChanged("Label");
                }
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
        /// Initializes a new instance of DoubleRange.
        /// </summary>
        public DoubleRange()
        {
            this.Min = Double.MinValue;
            this.Max = Double.MaxValue;

            RangeCollection.CollectionChanged += CollectionChanged;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Check if the components of two DoubleRanges are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(DoubleRange other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Check if the comopnents of two DoubleRanges are equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(DoubleRange a, DoubleRange b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        /// <summary>
        /// Check if a double value is valid with the given range.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(DoubleRange a, double value)
        {
            return a.IsValid(value);
        }

        /// <summary>
        /// Check if a double value is valid with this range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            try
            {
                value = System.Convert.ToDouble(value);
            }
            catch { }

            if (value.GetType() != typeof(double))
                throw new ArgumentException("The value needs to be double");

            var convert = Convert.ToDouble(value);

            if (convert < this._Min)
                return false;

            if (convert > this._Max)
                return false;

            return true;
        }

        /// <summary>
        /// Obtains a string that represents the DoubleRange.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}{2}{1}", this._Min, this._Max, Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Obtains a string that represents the DoubleRange.
        /// </summary>
        /// <param name="provider">An IFormatProvider doubleerface implementation that supplies culture-specific formatting information. </param>
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
