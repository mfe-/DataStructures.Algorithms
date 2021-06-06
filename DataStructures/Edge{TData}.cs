using System.Runtime.Serialization;
using System.Diagnostics;

namespace DataStructures
{
    /// <summary>
    /// Extends the <see cref="Edge"/> by a <see cref="Edge{TData}.Value"/> property.
    /// </summary>
    /// <typeparam name="TData">The type of the <seealso cref="Edge{TData}.Value"/> property</typeparam>
    [DebuggerDisplay("U={U}->V={V},Edge ={Weighted}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge<TData> : Edge, IEdge<TData>
    {
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
#pragma warning disable CS8601, CS8618 // Possible null reference assignment.
        public Edge(IVertex u, IVertex v) : base(u,v)
        {
            Value = default;
#pragma warning restore CS8601, CS8618 // Possible null reference assignment.
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        /// <param name="weighted">Sets the Weighted of the Edge</param>
#pragma warning disable CS8601, CS8618  // Possible null reference assignment.
        public Edge(IVertex u, IVertex v, double weighted) : base(u, v,weighted)
        {
            Value = default;
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        /// <inheritdoc/>
        public TData Value { get; set; }
    }
}
