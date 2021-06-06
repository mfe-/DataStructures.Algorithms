using System.Diagnostics;
using System.Runtime.Serialization;

namespace DataStructures.UI
{
    [DebuggerDisplay("U={U}->V={V},Edge ={Weighted},Value={Value}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge<TData> : Edge, IEdge<TData>
    {
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        public Edge(IVertex u, IVertex v) : base(u, v)
        {
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        /// <param name="weighted">Sets the Weighted of the Edge</param>
        public Edge(IVertex u, IVertex v, double weighted) : base(u, v, weighted)
        {
        }

        /// <summary>
        /// Get or sets the value of the edge
        /// </summary>
        [DataMember(Name = "Value", IsRequired = false)]
        public TData Value { get; set; }

    }
}
