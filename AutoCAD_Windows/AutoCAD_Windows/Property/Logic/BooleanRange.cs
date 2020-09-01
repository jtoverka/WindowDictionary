﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;

namespace WindowDictionary.Property.Logic
{
    /// <summary>
    /// Represents an booleger range of two boolegers.
    /// </summary>
    [XmlInclude(typeof(LogicalGate))]
    [Serializable]
    public class BooleanRange : Range, IEquatable<BooleanRange>, INotifyPropertyChanged
    {
        #region Properties

        private string _Label;
        /// <summary>
        /// Gets or Sets the label
        /// </summary>
        public override string Label
        {
            get { return _Label; }
            set
            {
                if (_Label == value)
                    return;

                _Label = value;
                OnPropertyChanged("Label");
            }
        }

        private bool _Min = false;
        /// <summary>
        /// Gets or Sets the Min Component.
        /// </summary>
        [XmlElement("Min")]
        public override object Min
        {
            get { return this._Min; }
            set
            {
                this._Min = Convert.ToBoolean(value);
            }
        }

        private bool _Max = true;

        /// <summary>
        /// Gets or Sets the Max Component.
        /// </summary>
        [XmlElement("Max")]
        public override object Max
        {
            get { return this._Max; }
            set
            {
                this._Max = Convert.ToBoolean(value);
            }
        }

        #endregion

        #region Functions

        /// <summary>
        /// Check if the components of two BooleanRanges are equal.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(BooleanRange other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Check if the comopnents of two BooleanRanges are equal.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(BooleanRange a, BooleanRange b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        /// <summary>
        /// Check if an booleger is valid with the given range.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValid(BooleanRange a, bool value)
        {
            return a.IsValid(value);
        }

        /// <summary>
        /// Check if an booleger is valid with this range.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            try
            {
                value = System.Convert.ToBoolean(value);
            }
            catch { }

            if (value.GetType() != typeof(bool))
                throw new ArgumentException("The value needs to be boolean");

            var convert = Convert.ToBoolean(value);

            return convert;
        }

        /// <summary>
        /// Obtains a string that represents the BooleanRange.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0}{2}{1}", this._Min, this._Max, Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }

        /// <summary>
        /// Obtains a string that represents the BooleanRange.
        /// </summary>
        /// <param name="provider">An IFormatProvider boolerface implementation that supplies culture-specific formatting information. </param>
        /// <returns>A string text.</returns>
        public string ToString(IFormatProvider provider)
        {
            return string.Format("{0}{2} {1}", this._Min.ToString(provider), this._Max.ToString(provider), Thread.CurrentThread.CurrentCulture.TextInfo.ListSeparator);
        }

        #endregion

        #region Delegates, Events, Handlers

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Invoke Propaerty Changed event
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
