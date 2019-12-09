using System.ComponentModel;

namespace DataStructures
{
    public interface IEdge : INotifyPropertyChanged
    {
        IVertex U { get; set; }
        IVertex V { get; set; }
        int Weighted { get; set; }
    }
}
