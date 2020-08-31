using System;
using System.Xml.Serialization;

namespace WindowDictionary.Property.Logic
{
    /// <summary>
    /// Represents the base class for all Range objects.
    /// </summary>
    [XmlInclude(typeof(BooleanRange))]
    [XmlInclude(typeof(CharRange))]
    [XmlInclude(typeof(DoubleRange))]
    [XmlInclude(typeof(IntegerRange))]
    [Serializable]
    public abstract class Range
    {
        #region Properties

        /// <summary>
        /// Holds the maximum value for range comparison.
        /// </summary>
        [XmlElement("Max")]
        public abstract object Max { get; set; }

        /// <summary>
        /// Holds the minimum value for range comparison.
        /// </summary>
        [XmlElement("Min")]
        public abstract object Min { get; set; }

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
