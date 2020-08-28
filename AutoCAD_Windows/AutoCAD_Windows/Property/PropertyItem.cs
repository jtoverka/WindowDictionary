using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a single property in a group
    /// </summary>
    [Serializable]
    public class PropertyItem : INotifyPropertyChanged
    {
        #region Properties

        private object _Parent = null;
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

        private string _PropertyName = "";
        /// <summary>
        /// Property Name to be displayed
        /// </summary>
        [XmlElement("PropertyName")]
        public string PropertyName
        {
            get { return this._PropertyName; }
            set
            {
                if (this._PropertyName == value)
                    return;

                this._PropertyName = value;
                OnPropertyChanged("PropertyName");
            }
        }

        private PropertyType _ValueType = PropertyType.String;
        /// <summary>
        /// Property type input
        /// </summary>
        [XmlElement("ValueType")]
        public PropertyType ValueType
        {
            get { return this._ValueType; }
            set
            {
                if (this._ValueType == value)
                    return;

                this._ValueType = value;
                OnPropertyChanged("ValueType");
            }
        }

        private Range _ValueRange = null;
        /// <summary>
        /// Restrictions for the value
        /// </summary>
        [XmlElement("ValueRange")]
        public Range ValueRange
        {
            get { return this._ValueRange; }
            set
            {
                if (this._ValueRange == value)
                    return;

                this._ValueRange = value;
                OnPropertyChanged("ValueRange");
            }
        }


        private int _ValueIndex = 0;
        /// <summary>
        /// Index of the Values collection
        /// </summary>
        [XmlElement("ValueIndex")]
        public int ValueIndex
        {
            get { return _ValueIndex; }
            set
            {
                if (_ValueIndex == value)
                    return;

                _ValueIndex = value;
                OnPropertyChanged("ValueIndex");
            }
        }

        /// <summary>
        /// Selection of values to choose from.
        /// </summary>
        [XmlElement("Values")]
        public ObservableCollection<object> Values { get; } = new ObservableCollection<object>();

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Invoked when a Component Property changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invokes PropertyChanged Event
        /// </summary>
        /// <param name="property"></param>
        private void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
