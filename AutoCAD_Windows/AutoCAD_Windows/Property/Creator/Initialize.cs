using System;
using System.Collections.ObjectModel;
using WindowDictionary.Property;
using WindowDictionary.Property.Creator;
using WindowDictionary.Property.Editor;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Provides the logic for creating properties
    /// </summary>
    public static class Initialize
    {
        /// <summary>
        /// Creates a property group with a child property group.
        /// This child property group can add sub-children to itself
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static PropertyGroup Group(string title)
        {
            PropertyType Properties;
            PropertyItem Property;

            // Create root Group
            var item = new PropertyGroup()
            {
                Title = title,
            };
            Properties = new PropertyType()
            {
                TypeSelected = PropertyTypes.CPropertyType,
                CPropertyType = Creator.CPropertyType.CGroup,
            };
            Property = new PropertyItem()
            {
                Type = Properties,
            };
            item.PropertyItems.Add(Property);

            #region Create Groups Collection

            var GGroups = new PropertyGroup()
            {
                Title = "Groups",
            };
            Properties = new PropertyType()
            {
                TypeSelected = PropertyTypes.CPropertyType,
                CPropertyType = Creator.CPropertyType.CGroupCollection,
            };
            Property = new PropertyItem()
            {
                Type = Properties,
            };

            GGroups.PropertyItems.Add(Property);

            #endregion

            item.PropertyGroups.Add(GGroups);

            #region Create Properties Collection

            var GProperties = new PropertyGroup()
            {
                Title = "Properties",
            };
            Properties = new PropertyType()
            {
                TypeSelected = PropertyTypes.CPropertyType,
                CPropertyType = Creator.CPropertyType.CPropertyCollection,
            };
            Property = new PropertyItem()
            {
                Type = Properties,
            };
            
            GProperties.PropertyItems.Add(Property);

            #endregion

            item.PropertyGroups.Add(GProperties);

            return item;
        }

        /// <summary>
        /// Create a group that can add children
        /// these children are groups that have only properties.
        /// These properties initialize the types of properties allowed 
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static PropertyGroup Property(string title)
        {
            PropertyType Properties;
            PropertyItem Property;

            // Create Property Group 
            var item = new PropertyGroup()
            {
                Title = title,
            };
            Properties = new PropertyType()
            {
                TypeSelected = PropertyTypes.CPropertyType,
                CPropertyType = CPropertyType.CProperty,
            };
            Property = new PropertyItem()
            {
                Type = Properties,
            };
            item.PropertyItems.Add(Property);

            return item;
        }
    }
}
