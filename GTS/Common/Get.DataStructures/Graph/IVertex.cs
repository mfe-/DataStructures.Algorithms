using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Get.DataStructure
{
    /// <summary>
    /// Vertex which is connected by edges 
    /// </summary>
    /// <typeparam name="W">A comparable type which is used for the weight of the vertex</typeparam>
    /// <typeparam name="D">The type of the data which the vertex contained</typeparam>
    public interface IVertex< W, D> : IData<D>
        where W : IComparable<W>  
    {
        /// <summary>
        /// Weight of vertex
        /// </summary>
        W Weight { get; set; }

        /// <summary>
        /// Outbound edges from vertex
        /// </summary>
        IEnumerable<IEdge<W, D>> Edges { get; set; }

        /// <summary>
        /// Amount of edges
        /// </summary>
        int Size { get; }

        /// <summary>
        /// Add an edge to the vertex
        /// </summary>
        /// <param name="U">The vertex which should be connected.</param>
        /// <param name="Weight">Weight of the edge.</param>
        /// <param name="Undirected">Determines whether the connection is directed or undirected.</param>
        /// <typeparamref name="Undirected">If <paramref name="Undirected"/>  is set to true, two edges will be created. The first edge will connect the current instance with the overgiven vertex <paramref name="U"/>. 
        /// The second edge will be created from vertex <paramref name="U"/> to the current instance.</typeparamref>
        /// <returns>The created edge</returns>
        IEdge<W, D> AddEdge(IVertex<W, D> U, W Weight, Boolean Undirected);

        /// <summary>
        /// Determines whether a directed or undirected edge should be deleted.
        /// </summary>
        /// <param name="U">The vertex to be removed.</param>
        /// <param name="Undirected">Determines whether an undirected or directed connection should be delted.</param>
        /// <typeparamref name="Undirected">If <paramref name="Undirected"/>  is set to true, two edges will be deleted. The first edge which is outgoing from vertex <paramref name="U"/> will be deleted. 
        /// The second edge from the current instance which connects the vertex <paramref name="U"/> will be deleted .</typeparamref>
        void RemoveEdge(IVertex<W, D> U, Boolean Undirected);
    }
}
