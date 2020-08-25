using System;
using System.Threading;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a string range with two characters
    /// </summary>
    public class StringRange : Range, IEquatable<StringRange>
    {
        #region Properties

        private char _Min;
        /// <summary>
        /// Gets or Sets the Min Component.
        /// </summary>
        public override object Min
        {
            get { return this._Min; }
            protected set
            {
                this._Min = Convert.ToChar(value);
            }
        }

        private char _Max;
        /// <summary>
        /// Gets or Sets the Max Component.
        /// </summary>
        public override object Max
        {
            get { return this._Max; }
            protected set
            {
                this._Max = Convert.ToChar(value);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of StringRange.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public StringRange(char min, char max)
        {
            this.Min = min;
            this.Max = max;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Check if the components of two StringRanges are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(StringRange other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Check if the comopnents of two StringRanges are equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals (StringRange a, StringRange b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        /// <summary>
        /// Check if a string is valid with this range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid (object value)
        {
            if (value.GetType() != typeof(string))
                throw new ArgumentException("The value needs to be string");

            var convert = Convert.ToString(value);

            char[] characters = convert.ToCharArray();
            foreach (char character in characters)
            {
                if (character < this._Min)
                    return false;

                if (character > this._Max)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Check if a string is valid with the given range.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid (StringRange a, string value)
        {
            return a.IsValid(value);
        }

        #endregion

        #region overrides

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