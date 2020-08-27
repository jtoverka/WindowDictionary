using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Serialization;
using System.Windows;
using System.Collections.Generic;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents a single property in a group
    /// </summary>
    [Serializable]
    public class PropertyItem : INotifyPropertyChanged
    {
        #region Properties

        private RoutedEventHandler _EventHandler;
        /// <summary>
        /// Allow an external method to be invoked by an external event
        /// </summary>
        [XmlIgnore]
        public RoutedEventHandler EventHandler
        {
            get { return this._EventHandler; }
            set
            {
                if (this._EventHandler == value)
                    return;

                this._EventHandler = value;
                OnPropertyChanged("EventHandler");
            }
        }

        private string _PropertyName;
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

        private PropertyType _ValueType;
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

        private Range _ValueRange;
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

        private object _Value;
        /// <summary>
        /// The value of the property
        /// </summary>
        [XmlElement("Value")]
        public object Value
        {
            get { return this._Value; }
            
            set
            {
                if (this._Value == value)
                    return;

                switch (this.ValueType)
                {
                    case PropertyType.Boolean:
                        try
                        {
                            value = System.Convert.ToBoolean(value);
                        }
                        catch (Exception) { }
                        if (value.GetType() != typeof(bool))
                            throw new ArgumentException("The value must be of type bool");
                        break;
                    case PropertyType.Double:
                        try
                        {
                            value = System.Convert.ToDouble(value);
                        }
                        catch (Exception) { }

                        if (value.GetType() != typeof(double))
                            throw new ArgumentException("The value must be of type double");
                        break;
                    case PropertyType.Integer:
                        try
                        {
                            value = System.Convert.ToInt32(value);
                        }
                        catch (Exception) { }

                        if (value.GetType() != typeof(int))
                            throw new ArgumentException("The value must be of type int");
                        break;
                    case PropertyType.SelectionString:
                        try
                        {
                            value = System.Convert.ToString(value);
                        }
                        catch (Exception) { }

                        if (value.GetType() != typeof(string))
                            throw new ArgumentException("The value must be of type string");

                        break;
                    case PropertyType.SelectionEditDouble:
                        try
                        {
                            value = System.Convert.ToDouble(value);
                        }
                        catch (Exception) { }

                        if (value.GetType() != typeof(double))
                            throw new ArgumentException("The value must be of type double");

                        break;
                    case PropertyType.SelectionEditInteger:
                        try
                        {
                            value = System.Convert.ToInt32(value);
                        }
                        catch (Exception) { }

                        if (value.GetType() != typeof(int))
                            throw new ArgumentException("The value must be of type int");

                        break;
                    case PropertyType.SelectionEditString:
                        try
                        {
                            value = System.Convert.ToString(value);
                        }
                        catch (Exception) { }

                        if (value.GetType() != typeof(string))
                            throw new ArgumentException("The value must be of type string");

                        break;
                    case PropertyType.String:
                        try
                        {
                            value = System.Convert.ToString(value);
                        }
                        catch (Exception) { }

                        if (value.GetType() != typeof(string))
                            throw new ArgumentException("The value must be of type string");
                        break;
                }

                this._Value = value;
                OnPropertyChanged("Value");
            }
        }

        /// <summary>
        /// Selection of values to choose from.
        /// </summary>
        [XmlElement("ValueSelection")]
        public ObservableCollection<object> ValueSelection { get; } = new ObservableCollection<object>();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public PropertyItem()
        {
            ValueRange = null;
            ValueType = PropertyType.String;
            Value = "";
        }

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        /// <param name="range"></param>
        /// <param name="type"></param>
        /// <param name="value"></param>
        public PropertyItem(Range range, PropertyType type, object value)
        {
            ValueRange = range;
            ValueType = type;
            Value = value;
        }

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
