using System;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents an integer range of two integers.
    /// </summary>
    public class IntegerRange : Range, IEquatable<IntegerRange>
    {
        #region Properties

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
                this._Min = Convert.ToInt32(value);
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
                this._Max = Convert.ToInt32(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of IntegerRange.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public IntegerRange(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Initializes a new instance of IntegerRange.
        /// </summary>
        public IntegerRange()
        {
            this.Min = int.MinValue;
            this.Max = int.MaxValue;
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
    }
}
