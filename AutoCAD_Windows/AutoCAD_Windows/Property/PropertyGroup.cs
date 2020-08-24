using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace WindowDictionary.Property
{
    public class PropertyGroup : INotifyPropertyChanged
    {
        #region Properties


        private string _Title = "Root";
        /// <summary>
        /// 
        /// </summary>
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

        public ObservableCollection<PropertyGroup> PropertyGroups { get; } = new ObservableCollection<PropertyGroup>();

        public ObservableCollection<PropertyItem> PropertyItems { get; } = new ObservableCollection<PropertyItem>();

        #endregion

        #region Delegates, Events, Handlers

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
