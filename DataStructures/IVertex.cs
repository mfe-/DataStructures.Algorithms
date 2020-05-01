using System;
using System.Collections.Generic;

namespace DataStructures
{
    public interface IVertex
    {
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        double Weighted { get; set; }
        ICollection<IEdge> Edges { get; }
        /// <summary>
        /// Creates a un/directed edge to the overgiven Vertex
        /// </summary>
        /// <param name="u">Vertex to connect</param>
        /// <param name="weighted">Weighted of the Edge</param>
        /// <param name="directed">False if the edge should be undirected (2 edges); othwise directed (1 edge)</param>
        IEdge AddEdge(IVertex u, double weighted = 0, bool directed = true);
        void RemoveEdge(IVertex u);
        void RemoveEdge(IVertex u, bool directed);
        /// <summary>
        /// Amount of neighbours
        /// </summary>
        IComparable Size { get; }
    }
}
