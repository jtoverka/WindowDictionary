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
        /// Traverse the tree to root, then collects all the property groups
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PropertyGroup> GetProperties();
    }
}
