using System;
using System.Runtime.InteropServices;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents the base class for all Range objects.
    /// </summary>
    public abstract class Range
    {
        #region Properties

        /// <summary>
        /// Holds the maximum value for range comparison.
        /// </summary>
        public abstract object Max { get; protected set; }

        /// <summary>
        /// Holds the minimum value for range comparison.
        /// </summary>
        public abstract object Min { get; protected set; }

        #endregion

        #region Functions

        /// <summary>
        /// Checkes if the value is within the range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract bool IsValid(object value);

        #endregion
    }
}
