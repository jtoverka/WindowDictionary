using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;
using WindowDictionary.Property.Editor;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a single property in a group
    /// </summary>
    [Serializable]
    public class PropertyItem : DependencyObject
    {
        #region Properties

        #region Property - CollectionRegex : string

        /// <summary>
        /// Gets or Sets the property dependency collection regular expression of this object.
        /// </summary>
        [XmlElement("CollectionRegex")]
        public string CollectionRegex
        {
            get { return (string)GetValue(CollectionRegexProperty); }
            set { SetValue(CollectionRegexProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CollectionRegex.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CollectionRegexProperty =
            DependencyProperty.Register("CollectionRegex", typeof(string), typeof(PropertyItem));

        #endregion
        #region Property - Name : string

        /// <summary>
        /// Gets the name of the parent title
        /// </summary>
        public string Name 
        { 
            get 
            {
                if (this.Parent == null)
                    return "";

                return this.Parent.Title;
            }
        }

        #endregion
        #region Property - Parent : PropertyGroup

        /// <summary>
        /// Gets or Sets the reference to the parent object.
        /// </summary>
        [XmlIgnore]
        public PropertyGroup Parent
        {
            get { return (PropertyGroup)GetValue(ParentProperty); }
            set { SetValue(ParentProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CollectionRegex.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty ParentProperty =
            DependencyProperty.Register("Parent", typeof(PropertyGroup), typeof(PropertyItem));

        #endregion
        #region Property - Regex : string

        /// <summary>
        /// Gets or Sets the property regular expression of this object.
        /// </summary>
        [XmlElement("Regex")]
        public string Regex
        {
            get { return (string)GetValue(RegexProperty); }
            set { SetValue(RegexProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CollectionRegex.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.Register("Regex", typeof(string), typeof(PropertyItem));

        #endregion
        #region Property - Type : PropertyType

        /// <summary>
        /// Gets or Sets the property type of this object.
        /// </summary>
        [XmlElement("Type")]
        public PropertyType Type
        {
            get { return (PropertyType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for CollectionRegex.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(PropertyType), typeof(PropertyItem));
        #endregion
        #region Property - Values : ObservableCollection<string>

        /// <summary>
        /// Gets the collection of values to choose from.
        /// </summary>
        [XmlElement("Values")]
        public ObservableCollection<string> Values { get; } = new ObservableCollection<string>();

        #endregion

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public PropertyItem()
        {
            this.CollectionRegex = "";
            this.Regex = ".*";
        }

        #endregion
    }
}