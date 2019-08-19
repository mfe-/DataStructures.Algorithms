using System;
using System.Diagnostics;

namespace Get.the.Solution.DataStructure
{
    [DebuggerDisplay("Edge = {Weight},U={U}, V = {V}")]
    public class Edge<W, D> : IEdge<W, D>
        where W : IComparable<W>
    {
        #region Members
        protected IVertex<W, D> u;
        protected IVertex<W, D> v;
        protected W weight;
        #endregion

        public Edge()
        {
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="pu">Vertex of the Edge</param>
        /// <param name="pv">Vertex of the Edge</param>
        public Edge(IVertex<W, D> pu, IVertex<W, D> pv)
            : this()
        {
            this.u = pu;
            this.v = pv;
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="pu">Vertex of the Edge</param>
        /// <param name="pv">Vertex of the Edge</param>
        /// <param name="pweighted">Sets the Weighted of the Edge</param>
        public Edge(IVertex<W, D> pu, IVertex<W, D> pv, W pweight)
            : this(pu, pv)
        {
            weight = pweight;
        }

        /// <summary>
        /// Get or sets the vertex U of the Edge
        /// </summary>
        public virtual IVertex<W, D> U { get { return u; } set { u = value; } }
        /// <summary>
        /// Get or sets the vertex V of the edge
        /// </summary>
        public virtual IVertex<W, D> V { get { return v; } set { v = value; } }
        /// <summary>
        /// Gets or sets the weight of the edge
        /// </summary>
        public virtual W Weight { get { return weight; } set { weight = value; } }

        public virtual D Value { get; set; }
        /// <summary>
        /// Determines whether two object instances are equal.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>True if the objects are considered equal; otherwise, false.</returns>
        public sealed override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(IEdge<W, D>))) return false;

            //true if objA is the same instance as objB or if both are null; otherwise, false.
            if (Object.ReferenceEquals(this, obj)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(this, null) || Object.ReferenceEquals(obj, null)) return false;

            IEdge<W, D> edge = obj as IEdge<W, D>;

            return Equals(edge, false);
        }

        /// <summary>
        /// Serves as a hash function for the type edge.
        /// The implementation of the GetHashCode method does not guarantee unique return values for different objects.
        /// The HasCode will be calculated with the GetHasCode functions from the vertex u and v. Transported edges have the same values.
        /// http://msdn.microsoft.com/en-us/library/system.object.gethashcode.aspx
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public sealed override int GetHashCode()
        {
            return Math.Abs(this.U.GetHashCode()) + Math.Abs(this.V.GetHashCode());
        }

    }
}
