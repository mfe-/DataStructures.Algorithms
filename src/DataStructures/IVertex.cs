using System;
using System.Collections.Generic;

namespace DataStructures
{
    /// <summary>
    /// <see cref="IVertex"/> can be used for creating a graph. Vertices are connected to each other using <see cref="IEdge"/>.
    /// </summary>
    public interface IVertex
    {
        /// <summary>
        /// Identifier of vertex
        /// </summary>
        Guid Guid { get; }
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        double Weighted { get; set; }
        /// <summary>
        /// A list of edges which connects the neigbour vertices
        /// </summary>
        ICollection<IEdge> Edges { get; }
        /// <summary>
        /// Creates a un/directed edge to the overgiven Vertex
        /// </summary>
        /// <param name="u">Vertex to connect</param>
        /// <param name="weighted">Weighted of the Edge</param>
        /// <param name="directed">False if the edge should be undirected (2 edges); othwise directed (1 edge)</param>
        IEdge AddEdge(IVertex u, double weighted = 0, bool directed = true);
        /// <summary>
        /// Removes the the edge from the current instance which contains a connection to the overgiven vertex <paramref name="u"/>
        /// </summary>
        /// <param name="u">The connecting vertex which should be removed</param>
        void RemoveEdge(IVertex u);
        /// <summary>
        /// Removes the the edge from the current instance which contains a connection to the overgiven vertex <paramref name="u"/>
        /// </summary>
        /// <param name="u">The connecting vertex which should be removed</param>
        /// <param name="directed">Determines whether the vertex is connected by a directed or undirected edge</param>
        void RemoveEdge(IVertex u, bool directed);
        /// <summary>
        /// Amount of neighbours
        /// </summary>
        IComparable Size { get; }
    }
}
