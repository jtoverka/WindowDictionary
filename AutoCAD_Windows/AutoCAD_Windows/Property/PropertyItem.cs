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
    public class PropertyItem : DependencyObject, ICloneable, INotifyPropertyChanged
    {
        #region Properties
        #region Property - Collapsible : bool

        /// <summary>
        /// Gets or Sets the visibility of the property. Collapsible if true, Gray out if false.
        /// </summary>
        public bool Collapsible
        {
            get { return (bool)GetValue(CollapsibleProperty); }
            set { SetValue(CollapsibleProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Collapsible.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty CollapsibleProperty =
            DependencyProperty.Register("Collapsible", typeof(bool), typeof(DependencyItem));

        #endregion
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
        #region Property - Help : string

        /// <summary>
        /// Gets or Sets the help text
        /// </summary>
        [XmlElement("Help")]
        public string Help
        {
            get { return (string)GetValue(HelpProperty); }
            set { SetValue(HelpProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for Help.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty HelpProperty =
            DependencyProperty.Register("Help", typeof(string), typeof(PropertyItem));

        #endregion
        #region Property - Name : string
        private string _Name;

        /// <summary>
        /// Gets or Sets the name of this property item
        /// </summary>
        public string Name
        {
            get 
            {
                if (_Name == null)
                {
                    if (Parent == null)
                        return "";

                    _Name = Parent.Title;
                }

                return _Name;
            }
            set 
            {
                if (value == _Name)
                    return;

                _Name = value;

                OnPropertyChanged("Name");
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
        #region Property - SelectedValue : string

        /// <summary>
        /// Gets or Sets the selected value from the values list
        /// </summary>
        public string SelectedValue
        {
            get { return (string)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        /// <summary>
        /// Using a DependencyProperty as the backing store for SelectedValue.  This enables animation, styling, binding, etc...
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(string), typeof(PropertyItem));

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
        [XmlArray("Values")]
        [XmlArrayItem("Value")]
        public ObservableCollection<string> Values { get; } = new ObservableCollection<string>();

        #endregion
        #region Property - DependencyItems : ObservableCollection<DependencyItem>

        /// <summary>
        /// Gets the collection of dependency items
        /// </summary>
        [XmlArray("DependencyItems")]
        [XmlArrayItem("DependencyItem")]
        public ObservableCollection<DependencyItem> DependencyItems { get; } = new ObservableCollection<DependencyItem>();

        #endregion
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public PropertyItem()
        {
            this.CollectionRegex = ".*";
            this.Regex = ".*";
            this.Help = "Any value is allowed.";
        }

        #endregion

        #region Methods

        /// <summary>
        /// Clones this object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            PropertyItem property = new PropertyItem()
            {
                Collapsible = this.Collapsible,
                CollectionRegex = this.CollectionRegex,
                Help = this.Help,
                Regex = this.Regex,
                Type = this.Type,
                SelectedValue = this.SelectedValue,
                Name = this.Name,
            };
            property.Type = this.Type.Clone() as PropertyType;
            foreach (string value in this.Values)
            {
                property.Values.Add(value);
            }
            foreach (DependencyItem item in this.DependencyItems)
            {
                property.DependencyItems.Add(item.Clone() as DependencyItem);
            }
            return property;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Raise event on this object property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}