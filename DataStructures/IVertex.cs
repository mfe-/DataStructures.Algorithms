using System.Collections.ObjectModel;
using System.ComponentModel;

namespace DataStructures
{
    public interface IVertex : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        int Weighted { get; set; }
        ObservableCollection<IEdge> Edges { get; set; }
        /// <summary>
        /// Creates a un/directed edge to the overgiven Vertex
        /// </summary>
        /// <param name="pu">Vertex to connect</param>
        /// <param name="pweighted">Weighted of the Edge</param>
        /// <param name="directed">False if the edge should be undirected (2 edges); othwise directed (1 edge)</param>
        IEdge AddEdge(IVertex pu, int pweighted = 0, bool directed = true);
        void RemoveEdge(IVertex pu);
        void RemoveEdge(IVertex pu, bool directed);
        /// <summary>
        /// Amount of neighbours
        /// </summary>
        int Size { get; }
    }
}
