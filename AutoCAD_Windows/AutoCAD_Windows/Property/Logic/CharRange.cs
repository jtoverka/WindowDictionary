using System;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property.Logic
{
    /// <summary>
    /// Represents a string range with two characters
    /// </summary>
    [Serializable]
    public class CharRange : Range, IEquatable<CharRange>
    {
        #region Properties

        private char _Min;
        /// <summary>
        /// Gets or Sets the Min Component.
        /// </summary>
        [XmlElement("Min")]
        public override object Min
        {
            get { return this._Min; }
            set
            {

                this._Min = value.ToString().ToCharArray()[0];
            }
        }

        private char _Max;
        /// <summary>
        /// Gets or Sets the Max Component.
        /// </summary>
        [XmlElement("Max")]
        public override object Max
        {
            get { return this._Max; }
            set
            {
                this._Max = value.ToString().ToCharArray()[0];
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of StringRange.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public CharRange(char min, char max)
        {
            this.Min = min;
            this.Max = max;
        }

        /// <summary>
        /// Initializes a new instance of StringRange.
        /// </summary>
        public CharRange()
        {
            this.Min = Char.MinValue;
            this.Max = Char.MaxValue;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Check if the components of two StringRanges are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(CharRange other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Check if the comopnents of two StringRanges are equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals (CharRange a, CharRange b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        /// <summary>
        /// Check if a string is valid with the given range.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid (CharRange a, string value)
        {
            return a.IsValid(value);
        }

        /// <summary>
        /// Check if a character is valid with this range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            try
            {
                value = System.Convert.ToChar(value);
            }
            catch { }

            if (value.GetType() != typeof(char))
                throw new ArgumentException("The value needs to be string");

            char character = System.Convert.ToChar(value);

            if (character < this._Min)
                return false;

            if (character > this._Max)
                return false;

            return true;
        }

        /// <summary>
        /// Obtains a string that represents the StringRange.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}{2}{1}",this.Min, this.Max, Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Obtains a string that represents the StringRange.
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