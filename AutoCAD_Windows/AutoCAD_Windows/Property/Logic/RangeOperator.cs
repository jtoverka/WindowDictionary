namespace WindowDictionary.Property.Logic
{
    /// <summary>
    /// Represents the different range objects allowed.
    /// </summary>
    public enum RangeOperator
    {
        /// <summary>
        /// A character minimum and maximum
        /// </summary>
        String,
        /// <summary>
        /// A double minimum and maximum
        /// </summary>
        Double,
        /// <summary>
        /// An integer minimum and maximum
        /// </summary>
        Integer,
    };
}