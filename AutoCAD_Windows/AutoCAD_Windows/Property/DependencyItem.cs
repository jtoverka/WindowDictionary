using System;
using System.Windows;
using System.Xml.Serialization;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a remote property to depend on
    /// </summary>
    public class DependencyItem : DependencyObject
    {
        #region Properties
        #region Property - Code : string

        /// <summary>
        /// Gets or Sets the property code
        /// </summary>
        [XmlElement("Code")]
        public string Code
        {
            get { return (string)GetValue(CodeProperty); }
            set { SetValue(CodeProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Code.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CodeProperty =
            DependencyProperty.Register("Code", typeof(string), typeof(DependencyItem));

        #endregion
        #region Property - Property : PropertyGroup

        /// <summary>
        /// Get or Set the property name to depend on
        /// </summary>
        [XmlElement("Property")]
        public PropertyGroup Property
        {
            get { return (PropertyGroup)GetValue(PropertyProperty); }
            set { SetValue(PropertyProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Property.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty PropertyProperty =
            DependencyProperty.Register("Property", typeof(PropertyGroup), typeof(DependencyItem));

        #endregion
        #region Property - Regex : string

        /// <summary>
        /// Gets or Sets the regular expression for a dependency match
        /// </summary>
        [XmlElement("Regex")]
        public string Regex
        {
            get { return (string)GetValue(RegexProperty); }
            set { SetValue(RegexProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Regex.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty RegexProperty =
            DependencyProperty.Register("Regex", typeof(string), typeof(DependencyItem));


        #endregion
        #endregion
    }
}
