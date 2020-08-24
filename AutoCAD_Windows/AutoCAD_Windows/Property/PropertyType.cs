
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
        SelectionEditStringAll,

        /// <summary>
        /// Displays Property Item as a ComboBox. This does allow ComboBox Editing for a String input, this does not allow special characters.
        /// </summary>
        SelectionEditStringNoSpecial,

        /// <summary>
        /// Displays Property Item as a TextBox for a String input.
        /// </summary>
        StringAll,

        /// <summary>
        /// Displays Property Item as a TextBox for a String input that does not allow special characters.
        /// </summary>
        StringNoSpecial,
    }
}
