using System.Collections.ObjectModel;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Provides an interface for controlling properties
    /// </summary>
    public interface IPropertyControl
    {
        /// <summary>
        /// Property PropertyGroups 
        /// </summary>
        public ObservableCollection<PropertyGroup> PropertyGroups { get; }

        /// <summary>
        /// Gets the tree path to this group
        /// </summary>
        public string Path { get; }

        /// <summary>
        /// Traverse the tree to root, then collects all the property groups
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PropertyGroup> GetProperties();

        /// <summary>
        /// Determines if the name provided is unique within the property groups.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsPropertyGroupUnique(string name);
    }
}
