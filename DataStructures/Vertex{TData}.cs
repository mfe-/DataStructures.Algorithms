using System.Runtime.Serialization;
using System.Diagnostics;

namespace DataStructures
{
    [DebuggerDisplay("Vertex={Weighted},Value={Value},GUID={_Guid}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Vertex")]
    public class Vertex<TData> : Vertex, IVertex<TData>
    {
        /// <summary>
        /// Initializes a new instance of the Vertex class.
        /// </summary>
        public Vertex() : base()
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            Value = default;
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        /// <summary>
        /// Initializes a new instance of the Vertex class that contains the specified weighted.
        /// </summary>
        /// <param name="weighted"></param>
        public Vertex(double weighted)
            : base(weighted)
        {
#pragma warning disable CS8601 // Possible null reference assignment.
            Value = default;
#pragma warning restore CS8601 // Possible null reference assignment.
        }
        [DataMember(Name = "Value", Order = 0, IsRequired = false)]
        public TData Value { get; set; }

        public override IEdge CreateEdge(IVertex u, double weighted = 0)
        {
            IEdge e1 = new Edge<TData>(this, u, weighted);
            return e1;
        }

    }
}