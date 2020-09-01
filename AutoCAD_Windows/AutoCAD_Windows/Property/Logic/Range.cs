using System;
using System.Collections.ObjectModel;
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
        /// Gets or Sets the Parent Range object
        /// </summary>
        [XmlIgnore]
        public abstract Object Parent { get; set; }
        /// <summary>
        /// Gets the Range Collection
        /// </summary>
        [XmlElement("RangeCollection")]
        public abstract ObservableCollection<Range> RangeCollection { get; }
        /// <summary>
        /// Gets or Sets the Name of the Range
        /// </summary>
        [XmlElement("Label")]
        public abstract string Label { get; }
        
        /// <summary>
        /// Gets or Sets the maximum value for range comparison.
        /// </summary>
        [XmlElement("Max")]
        public abstract object Max { get; set; }

        /// <summary>
        /// Gets or Sets the minimum value for range comparison.
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
