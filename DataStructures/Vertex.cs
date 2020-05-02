using System.Runtime.Serialization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;

namespace DataStructures
{
    [DebuggerDisplay("Vertex={Weighted},GUID={_Guid}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Vertex")]
    public class Vertex : IVertex
    {
        [DataMember(Name = "Guid", Order = 3, IsRequired = true)]
        private readonly Guid _Guid;

        /// <summary>
        /// Initializes a new instance of the Vertex class.
        /// </summary>
        public Vertex()
        {
            _Guid = Guid.NewGuid();
            Edges = new List<IEdge>(10);
        }
        public Vertex(ICollection<IEdge> edges) : this()
        {
            _Guid = Guid.NewGuid();
            Edges = edges;
        }

        /// <summary>
        /// Initializes a new instance of the Vertex class that contains the specified weighted.
        /// </summary>
        /// <param name="weighted"></param>
        public Vertex(double weighted)
            : this()
        {
            Weighted = weighted;
        }
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        [DataMember(Name = "Weighted", Order = 1, IsRequired = true)]
        public virtual double Weighted { get; set; }

        /// <summary>
        /// Gets or sets the list of edges which connects the vertex neighbours
        /// </summary>
        [DataMember(Name = "Edges", Order = 2, IsRequired = true)]
        public virtual ICollection<IEdge> Edges { get; protected set; }

        /// <summary>
        /// Amount of neighbours
        /// </summary>
        public IComparable Size
        {
            get
            {
                return Edges.Count;
            }
        }
        /// <inheritdoc/>
        public Guid Guid
        {
            get
            {
                return _Guid;
            }
        }
        public virtual IEdge CreateEdge(IVertex u, double weighted = 0)
        {
            IEdge e1 = new Edge(this, u, weighted);
            return e1;
        }
        /// <summary>
        /// Creates a (un)directed edge to the overgiven Vertex
        /// </summary>
        /// <param name="u">Vertex to connect</param>
        /// <param name="weighted">Weighted of the Edge</param>
        /// <param name="directed">False if the edge should be undirected (2 edges); othwise directed (1 edge)</param>
        public virtual IEdge AddEdge(IVertex u, double weighted = 0, bool directed = true)
        {
            IEdge e1 = CreateEdge(u, weighted);
            Edges.Add(e1);
            if (!directed)
            {
                u?.AddEdge(this, weighted, true);
            }
            return Edges.Last();
        }

        public virtual void RemoveEdge(IVertex u)
        {
            RemoveEdge(u, true);
        }
        public virtual void RemoveEdge(IVertex u, bool directed)
        {
            IEdge edge = this.Edges.FirstOrDefault(a => a.U.Equals(this) && a.V.Equals(u));
            if (edge != null)
            {
                if (directed.Equals(false))
                {
                    IEdge edged = edge.V.Edges.FirstOrDefault(a => a.U.Equals(edge.V) && a.V.Equals(this) && a.Weighted.Equals(edge.Weighted));

                    edge.V.Edges.Remove(edged);
                }
                this.Edges.Remove(edge);
            }
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return _Guid.ToString();
        }
        /// <summary>
        /// Determines with the guid whether the specified Object is equal to the current Object.
        /// http://msdn.microsoft.com/en-us/library/bsc2ak47.aspx
        /// </summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj != null && !obj.GetType().Equals(this.GetType())) return false;

            return this._Guid.Equals((obj as Vertex)?._Guid);
        }
        /// <summary>
        /// Returns the Hashvalue for this typ based on the internal used guid
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return _Guid.GetHashCode();
        }

    }
}