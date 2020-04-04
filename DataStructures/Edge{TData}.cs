using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics;
using System;

namespace DataStructures
{
    [DebuggerDisplay("U={U}->V={V},Edge ={Weighted}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge<TData> : Edge, IEdge<TData>
    {
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        public Edge(IVertex u, IVertex v) : base(u,v)
        {
            Value = default;
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        /// <param name="weighted">Sets the Weighted of the Edge</param>
        public Edge(IVertex u, IVertex v, int weighted) : base(u, v,weighted)
        {
            Value = default;
        }
        public TData Value { get; set; }
    }
}
