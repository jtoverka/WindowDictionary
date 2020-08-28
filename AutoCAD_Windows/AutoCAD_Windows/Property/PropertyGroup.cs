using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a group of property items and other property groups
    /// </summary>
    [Serializable]
    public class PropertyGroup : INotifyPropertyChanged
    {
        #region Properties

        private object _Parent;
        /// <summary>
        /// Holds reference to the parent object
        /// </summary>
        [XmlIgnore]
        public object Parent
        {
            get { return _Parent; }
            set
            {
                if (_Parent == value)
                    return;

                _Parent = value;
                OnPropertyChanged("Parent");
            }
        }

        private string _Title = "Root";
        /// <summary>
        /// Displays the title of the group
        /// </summary>
        [XmlElement("Title")]
        public string Title
        {
            get { return this._Title; }
            set
            {
                if (this._Title == value)
                    return;

                this._Title = value;
                OnPropertyChanged("Title");
            }
        }

        /// <summary>
        /// Collection of other <see cref="PropertyGroup">Property Groups</see> in a hierarchy
        /// </summary>
        [XmlElement("PropertyGroups")]
        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();

        /// <summary>
        /// Collection of <see cref="PropertyItem">Property Items</see> within this group
        /// </summary>
        [XmlElement("PropertyItems")]
        public ObservableCollection<PropertyItem> PropertyItems { get; } = new ObservableCollection<PropertyItem>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public PropertyGroup()
        {
            PropertyGroups.CollectionChanged += PropertyGroups_CollectionChanged;
            PropertyItems.CollectionChanged += PropertyItems_CollectionChanged;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked on Property Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        private void PropertyItems_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<PropertyItem>;
            if ((e.NewItems.Count > 0) && (collection != null))
            {
                foreach (PropertyItem item in collection)
                {
                    item.Parent = this;
                    OnPropertyChanged("PropertyItems");
                }
            }
        }

        private void PropertyGroups_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var collection = sender as ObservableCollection<PropertyGroup>;
            if ((e.NewItems?.Count > 0) && (collection != null))
            {
                foreach (PropertyGroup item in collection)
                {
                    item.Parent = this;
                    OnPropertyChanged("PropertyGroups");
                }
            }
        }

        #endregion
    }
}
