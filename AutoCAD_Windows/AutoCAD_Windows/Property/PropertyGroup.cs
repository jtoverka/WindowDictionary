﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
        
        private string _Title = "Root";
        /// <summary>
        /// Displays the title of the group
        /// </summary>
        [XmlAttribute("Title")]
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
        
        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked on Property Changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
