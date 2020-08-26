namespace WindowDictionary.Property
{
    /// <summary>
    /// Property Item Type
    /// </summary>
    public enum PropertyType
    {
        /// <summary>
        /// Displays Property Item as a CheckBox
        /// </summary>
        Boolean,

        /// <summary>
        /// Displays property item as a button
        /// </summary>
        Button,

        /// <summary>
        /// Displays Property Item as a TextBox that prevents Cut, Copy, Paste, and improper key input. Only Allows number Double input.
        /// </summary>
        Double,

        /// <summary>
        /// Displays Property Item as a TextBox that prevents Cut, Copy, Paste, and improper key input. Only Allows number Integer input.
        /// </summary>
        Integer,

        /// <summary>
        /// Displays Property Item as a ComboBox. This does not allow ComboBox Editing.
        /// </summary>
        SelectionString,

        /// <summary>
        /// Displays Property Item as a ComboBox. This does allow ComboBox Editing for a number Double input.
        /// </summary>
        SelectionEditDouble,

        /// <summary>
        /// Displays Property Item as a ComboBox. This does allow ComboBox Editing for a number Integer input.
        /// </summary>
        SelectionEditInteger,

        /// <summary>
        /// Displays Property Item as a ComboBox. This does allow ComboBox Editing for a String input.
        /// </summary>
        SelectionEditString,

        /// <summary>
        /// Displays Property Item as a TextBox for a String input.
        /// </summary>
        String,
    }
}
