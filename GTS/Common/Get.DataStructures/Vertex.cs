using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Get.DataStructure
{
    [DebuggerDisplay("Edge = {Weighted},U={U}, V = {V}")]
    public class Vertex<W> : IVertex<W>
        where W : IComparable<W>
    {

        #region Members
        protected IEnumerable<IEdge<W>> _Edges;
        protected Guid _Guid;
        protected W weight;
        #endregion

        public Vertex()
        {
            this._Guid = Guid.NewGuid();
            this._Edges = new List<IEdge<W>>();
        }

        /// <summary>
        /// Initializes a new instance of the Vertex class that contains the specified weighted.
        /// </summary>
        /// <param name="pweighted"></param>
        public Vertex(W weight)
            : this()
        {
            this.weight = weight;
        }
        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        public virtual W Weight { get { return this.weight; } set { this.weight = value; } }

        /// <summary>
        /// Gets or sets the list of edges which connects the vertex neighbours
        /// </summary>
        public IEnumerable<IEdge<W>> Edges { get { return _Edges; } set { _Edges = value; } }

        /// <summary>
        /// Amount of neighbours
        /// </summary>
        public int Size
        {
            get
            {
                return Edges.Count(); //Knotengrad
            }
        }



        public virtual IEdge<W> AddEdge(IVertex<W> U, W Weight, bool Undirected)
        {
            IEdge<W> e1 = new Edge<W>(this, U, Weight);
            _Edges.ToList<IEdge<W>>().Add(e1);
            if (Undirected == true)
            {
                U.AddEdge(this, Weight, false);
            }

            return e1;
        }

        public virtual void RemoveEdge(IVertex<W> U, bool Undirected)
        {
            IEdge<W> edge = this.Edges.Where(a => a.U.Equals(this) && a.V.Equals(U)).FirstOrDefault<IEdge<W>>();

            if (Undirected.Equals(false))
            {
                IEdge<W> edged = edge.V.Edges.Where(a => a.U.Equals(edge.V) && a.V.Equals(this) && a.Weight.Equals(edge.Weight)).FirstOrDefault<IEdge<W>>();

                edge.V.Edges.ToList<IEdge<W>>().Remove(edged);
            }
            this.Edges.ToList<IEdge<W>>().Remove(edge);
        }

        /// <summary>
        /// Determines with the guid whether the specified Object is equal to the current Object.
        /// http://msdn.microsoft.com/en-us/library/bsc2ak47.aspx
        /// </summary>
        /// <param name="obj">The object to compare with the current object. </param>
        /// <returns>true if the specified Object is equal to the current Object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(IVertex<W>))) return false;

            return this._Guid.Equals((obj as Vertex<W>)._Guid);
        }
        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return _Guid.GetHashCode();
        }
    }
}
