﻿using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using WindowDictionary.Property.Editor;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a group of property items and other property groups
    /// </summary>
    [Serializable]
    public class PropertyGroup : INotifyPropertyChanged, IPropertyControl, ICloneable
    {
        #region Fields

        private IPropertyControl _Parent;
        private string _Title = "Root";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the tree path
        /// </summary>
        [XmlIgnore]
        public string Path
        {
            get 
            {
                if (this.Parent == null)
                    return this.Title;
                else
                    return this.Parent.Path + "/" + this.Title;
            }
        }
        /// <summary>
        /// Gets or Sets the reference to the parent object
        /// </summary>
        [XmlIgnore]
        public IPropertyControl Parent
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

        /// <summary>
        /// Gets or Sets the title of the group
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
        /// Gets the Collection of other <see cref="PropertyGroup">Property Groups</see> in a hierarchy
        /// </summary>
        [XmlArray("PropertyGroups")]
        [XmlArrayItem("PropertyGroup")]
        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();

        /// <summary>
        /// Gets the Collection of <see cref="PropertyItem">Property Items</see> within this group
        /// </summary>
        [XmlArray("PropertyItems")]
        [XmlArrayItem("PropertyItem")]
        public ObservableCollection<PropertyItem> PropertyItems { get; } = new ObservableCollection<PropertyItem>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class.
        /// </summary>
        public PropertyGroup()
        {
            PropertyGroups.CollectionChanged += PropertyGroups_CollectionChanged;
            PropertyItems.CollectionChanged += PropertyItems_CollectionChanged;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns all of the properties in the parent tree.
        /// </summary>
        /// <returns></returns>
        public ObservableCollection<PropertyGroup> GetProperties()
        {
            return this.Parent.GetProperties();
        }

        /// <summary>
        /// Returns false if the string provided exists in the property group collection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsPropertyGroupUnique(string name)
        {
            foreach (PropertyGroup item in this.PropertyGroups)
            {
                if (item.Title == name)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Clones this object
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            PropertyGroup group = new PropertyGroup()
            {
                Title = this.Title,
            };
            foreach (PropertyGroup item in this.PropertyGroups)
            {
                group.PropertyGroups.Add(item.Clone() as PropertyGroup);
            }
            foreach(PropertyItem item in this.PropertyItems)
            {
                group.PropertyItems.Add(item.Clone() as PropertyItem);
            }
            return group;
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked on Property Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="property"></param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Sets the parent of all items added to the <see cref="PropertyItems"/> collection to this object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems?.Count > 0)
            {
                foreach (PropertyItem item in e.NewItems)
                {
                    item.Parent = this;
                }
                OnPropertyChanged("PropertyItems");
            }
        }

        /// <summary>
        /// Sets the parent of all items added to the <see cref="PropertyItems"/> collection to this object.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PropertyGroups_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems?.Count > 0)
            {
                foreach (PropertyGroup item in e.NewItems)
                {
                    item.Parent = this;
                }
                OnPropertyChanged("PropertyGroups");
            }
        }

        #endregion
    }
}
