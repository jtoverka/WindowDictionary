using System;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace WindowDictionary.Property
{
    /// <summary>
    /// Represents 
    /// </summary>
    public class PropertyItem : INotifyPropertyChanged
    {
        #region Properties


        private string _PropertyName;
        /// <summary>
        /// 
        /// </summary>
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
        /// 
        /// </summary>
        public PropertyType ValueType
        {
            get { return this._ValueType; }
            protected set
            {
                if (this._ValueType == value)
                    return;

                this._ValueType = value;
                OnPropertyChanged("ValueType");
            }
        }

        private Range _ValueRange;
        /// <summary>
        /// 
        /// </summary>
        public Range ValueRange
        {
            get { return this._ValueRange; }
            protected set
            {
                if (this._ValueRange == value)
                    return;

                this._ValueRange = value;
                OnPropertyChanged("ValueRange");
            }
        }

        private object _Value;
        /// <summary>
        /// 
        /// </summary>
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
                        if (value.GetType() != typeof(string))
                            throw new ArgumentException("The value must be of type string");
                        break;
                    case PropertyType.SelectionEditDouble:
                        if (value.GetType() != typeof(ObservableCollection<double>))
                            throw new ArgumentException("The value must be of type ObservableCollection<double>");
                        break;
                    case PropertyType.SelectionEditInteger:
                        if (value.GetType() != typeof(ObservableCollection<int>))
                            throw new ArgumentException("The value must be of type ObservableCollection<int>");
                        break;
                    case PropertyType.SelectionEditStringAll:
                        if (value.GetType() != typeof(ObservableCollection<string>))
                            throw new ArgumentException("The value must be of type ObservableCollection<string>");
                        break;
                    case PropertyType.SelectionEditStringNoSpecial:
                        if (value.GetType() != typeof(ObservableCollection<string>))
                            throw new ArgumentException("The value must be of type ObservableCollection<string>");
                        break;
                    case PropertyType.StringAll:
                        if (value.GetType() != typeof(string))
                            throw new ArgumentException("The value must be of type string");
                        break;
                    case PropertyType.StringNoSpecial:
                        if (value.GetType() != typeof(string))
                            throw new ArgumentException("The value must be of type string");
                        break;
                }

                this._Value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region Constructors
        
        /// <summary>
        /// 
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
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
