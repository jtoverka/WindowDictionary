using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using WindowDictionary.Property.Logic;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a single property in a group
    /// </summary>
    [Serializable]
    public class PropertyItem : INotifyPropertyChanged
    {
        #region Fields

        private object _Parent = null;
        private string _PropertyName = "";
        private Range _ValueRange = new LogicalGate(LogicalOperator.OR);
        private PropertyType _ValueType = PropertyType.String;
        private int _ValueIndex = 0;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or Sets the reference to the parent object.
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

        /// <summary>
        /// Gets or Sets the property name of this object.
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

        /// <summary>
        /// Gets or Sets the property type input.
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

        /// <summary>
        /// Gets or Sets the restriction for this object.
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


        /// <summary>
        /// Gets or Sets the index of the values collection.
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
        /// Gets the collection of values to choose from.
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