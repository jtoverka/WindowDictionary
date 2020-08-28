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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of this class
        /// </summary>
        public PropertyItem()
        {
            Values.CollectionChanged += Values_CollectionChanged;
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

        /// <summary>
        /// Checks to see if the value stored in the collection is valid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Values_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            for (int i = 0; i < e.NewItems.Count; i++)
            {
                object value = e.NewItems[i];

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
                OnPropertyChanged("Values");
            }
        }

        #endregion
    }
}
