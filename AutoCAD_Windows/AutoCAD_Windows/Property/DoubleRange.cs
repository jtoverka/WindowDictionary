using System;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a double range of two doubles.
    /// </summary>
    [Serializable]
    public class DoubleRange : Range, IEquatable<DoubleRange>
    {
        #region Properties

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
                this._Min = Convert.ToDouble(value);
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
                this._Max = Convert.ToDouble(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of DoubleRange.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public DoubleRange(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Initializes a new instance of DoubleRange.
        /// </summary>
        public DoubleRange()
        {
            this.Min = Double.MinValue;
            this.Max = Double.MaxValue;
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
    }
}
