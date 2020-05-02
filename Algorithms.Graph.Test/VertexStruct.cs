using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Algorithms.Graph.Test
{
    public struct Vertex : IVertex
    {
        public readonly Guid _Guid;

        public Guid Guid
        {
            get
            {
                return _Guid;
            }
        }

        public Vertex(ICollection<IEdge> edges) : this(0)
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
            _Guid = Guid.NewGuid();
            Edges = new List<IEdge>(10);
        }
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        public double Weighted { get; set; }

        /// <summary>
        /// Gets or sets the list of edges which connects the vertex neighbours
        /// </summary>
        public ICollection<IEdge> Edges { get; }

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

        public IEdge CreateEdge(IVertex u, double weighted = 0)
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
        public IEdge AddEdge(IVertex u, double weighted = 0, bool directed = true)
        {
            IEdge e1 = CreateEdge(u, weighted);
            Edges.Add(e1);
            if (!directed)
            {
                u?.AddEdge(this, weighted, true);
            }
            return Edges.Last();
        }

        public void RemoveEdge(IVertex u)
        {
            RemoveEdge(u, true);
        }
        public void RemoveEdge(IVertex u, bool directed)
        {
            Vertex vertex = this;
            IEdge edge = this.Edges.FirstOrDefault(a => a.U.Equals(vertex) && a.V.Equals(u));
            if (edge != null)
            {
                if (directed.Equals(false))
                {
                    IEdge edged = edge.V.Edges.FirstOrDefault(a => a.U.Equals(edge.V) && a.V.Equals(vertex) && a.Weighted.Equals(edge.Weighted));

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

            return this._Guid.Equals(((Vertex)(obj))._Guid);
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
