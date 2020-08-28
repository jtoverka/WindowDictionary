using System.ComponentModel;

namespace WindowDictionary.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public class DxfLayerExtended : netDxf.Tables.Layer, INotifyPropertyChanged
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public DxfLayerExtended(string name) : base(name) { }

        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
