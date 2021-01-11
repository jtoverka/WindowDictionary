namespace WindowDictionary.Property.Editor
{
    /// <summary>
    /// Property Item Type
    /// </summary>
    public enum EPropertyType
    {
        /// <summary>
        /// Displays Property Item as a CheckBox.
        /// </summary>
        CheckBox,

        /// <summary>
        /// Displays Property Item as a ComboBox. This does not allow ComboBox Editing.
        /// </summary>
        ComboBox,

        /// <summary>
        /// Displays Property Item as a ComboBox. This does allow ComboBox Editing for an input.
        /// </summary>
        ComboBoxEdit,

        /// <summary>
        /// Displays Property Item as a TextBox for an input.
        /// </summary>
        TextBox,
    }
}
