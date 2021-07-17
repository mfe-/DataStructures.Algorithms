using System.Runtime.Serialization;
using System.Diagnostics;

namespace DataStructures
{
    /// <summary>
    /// Extends the <see cref="Vertex"/> by a <see cref="Vertex{TData}.Value"/> property.
    /// </summary>
    /// <typeparam name="TData">The type of the <seealso cref="Vertex{TData}.Value"/> property</typeparam>
    [DebuggerDisplay("Vertex={Weighted},Value={Value},GUID={_Guid}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Vertex")]
    public class Vertex<TData> : Vertex, IVertex<TData>
    {
        /// <summary>
        /// Initializes a new instance of the Vertex class.
        /// </summary>
#pragma warning disable CS8618, CS8601 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Vertex() : base()
        {
            Value = default;
#pragma warning restore CS8601, CS8618 // Possible null reference assignment.
        }

        /// <summary>
        /// Initializes a new instance of the Vertex class that contains the specified weighted.
        /// </summary>
        /// <param name="weighted"></param>
#pragma warning disable CS8618, CS8601 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Vertex(double weighted)
            : base(weighted)
        {
            Value = default;
        }
#pragma warning restore CS8601, CS8618 // Possible null reference assignment.
        /// <inheritdoc/>
        [DataMember(Name = "Value", Order = 0, IsRequired = false)]
        public TData Value { get; set; }

        /// <inheritdoc/>
        public override Edge<TData> CreateEdge(IVertex u, double weighted = 0) => new Edge<TData>(this, u, weighted);

    }
}