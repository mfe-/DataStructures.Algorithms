using System;

namespace Get.the.Solution.DataStructure
{
    /// <summary>
    /// Represents a Ede which holdes the two connected vertices u and v
    /// </summary>
    /// <typeparam name="W">A compareable type for the edge weight</typeparam>
    /// <typeparam name="D">The Data Type of the vertices</typeparam>
    public interface IEdge<W, D> : IData<D>
        where W : IComparable<W>
    {
        /// <summary>
        /// Weight of vertex
        /// </summary>
        W Weight { get; set; }
        /// <summary>
        /// Get or sets the Vertex U
        /// </summary>
        IVertex<W, D> U { get; set; }
        /// <summary>
        /// Get or sets the Vertex V
        /// </summary>
        IVertex<W, D> V { get; set; }
    }
}
