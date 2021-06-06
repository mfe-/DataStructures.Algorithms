using System.Runtime.Serialization;
using System.Diagnostics;
using System;

namespace DataStructures
{
    /// <summary>
    /// An edge that connects vertices <see cref="IEdge.U"/> and <see cref="IEdge.V"/>
    /// </summary>
    [DebuggerDisplay("U={U}->V={V},Edge={Weighted}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge : IEdge
    {
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        public Edge(IVertex u, IVertex v)
        {
            U = u;
            V = v;
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        /// <param name="weighted">Sets the Weighted of the Edge</param>
        public Edge(IVertex u, IVertex v, double weighted) : this(u, v)
        {
            Weighted = weighted;
        }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "U", Order = 2, IsRequired = true)]
        public virtual IVertex U { get; set; }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "V", Order = 3, IsRequired = true)]
        public virtual IVertex V { get; set; }
        /// <summary>
        /// Gets or sets the Weighted of the Edge
        /// </summary>
        [DataMember(Name = "Weighted", IsRequired = true)]
        public virtual double Weighted { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{U} -> {V}";
        }
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is IEdge)) return false;
            IEdge edge = (IEdge)obj;
            return ($"{U}{V}" == $"{edge.U}{edge.V}" || $"{U}{V}" == $"{edge.V}{edge.U}");
        }

        /// <summary>
        /// Creates a HasCode based of the used vertices. If no Vertex is set the value zero is used.
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return Math.Abs(U.GetHashCode()) + (V != null ? Math.Abs(V.GetHashCode()) : 0);
        }
    }
}
