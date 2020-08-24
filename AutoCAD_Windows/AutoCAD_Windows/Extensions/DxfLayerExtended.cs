using System.ComponentModel;

namespace WindowDictionary.Extensions
{
    public class DxfLayerExtended : netDxf.Tables.Layer, INotifyPropertyChanged
    {
        public DxfLayerExtended(string name) : base(name) { }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
