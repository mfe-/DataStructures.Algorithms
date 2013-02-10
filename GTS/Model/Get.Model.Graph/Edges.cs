using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Get.Model.Graph
{
    [DebuggerDisplay("Edge = {Weighted},U={U}, V = {V}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge : INotifyPropertyChanged, IEqualityComparer<Edge>
    {
        #region Members
        protected Vertex u;
        protected Vertex v;

        protected int weighted;
        #endregion

        /// <summary>
        /// Initializes a new instance of the Edge class for using the IEqualityComparer on Distinct
        /// </summary>
        public Edge() { }

        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="pu">Vertex of the Edge</param>
        /// <param name="pv">Vertex of the Edge</param>
        public Edge(Vertex pu, Vertex pv)
        {
            u = pu;
            v = pv;
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="pu">Vertex of the Edge</param>
        /// <param name="pv">Vertex of the Edge</param>
        /// <param name="pweighted">Sets the Weighted of the Edge</param>
        public Edge(Vertex pu, Vertex pv, int pweighted)
        {
            u = pu;
            v = pv;
            weighted = pweighted;
        }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "U", Order = 2, IsRequired = true)]
        public Vertex U { get { return u; } set { u = value; NotifyPropertyChanged("U"); } }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "V", Order = 3, IsRequired = true)]
        public Vertex V { get { return v; } set { v = value; NotifyPropertyChanged("V"); } }
        /// <summary>
        /// Gets or sets the Weighted of the Edge
        /// </summary>
        [DataMember(Name = "Weighted", IsRequired = true)]
        public int Weighted { get { return weighted; } set { weighted = value; NotifyPropertyChanged("Weighted"); } }
        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return base.ToString() + string.Empty + U.ToString() + " -> " + V.ToString();
        }

        public override bool Equals(object obj)
        {
            if (!obj.GetType().Equals(typeof(Edge))) return false;

            //true if objA is the same instance as objB or if both are null; otherwise, false.
            if (Object.ReferenceEquals(this, obj)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(this, null) || Object.ReferenceEquals(obj, null)) return false;

            Edge edge = obj as Edge;

            return Equals(edge, false);
        }
        /// <summary>
        /// Returns a value indicating whether this instance is equal to the edge.
        /// </summary>
        /// <param name="edge">The edge to compare to this instance.</param>
        /// <param name="permute">If the parameter is true transported edges will be handled as equal</param>
        /// <returns>True if the instance and the overgiven edge are euqa; otherwiese, false.</returns>
        protected virtual bool Equals(Edge edge, bool permute)
        {
            if (permute)
            {
                if (!this.U.Equals(edge.V)) return false;
                if (!this.V.Equals(edge.U)) return false;
                if (!this.Weighted.Equals(edge.Weighted)) return false;
                if (!this.GetHashCode().Equals(edge.GetHashCode())) return false;
                return true;
            }
            else
            {
                if (!this.U.Equals(edge.U)) return false;
                if (!this.V.Equals(edge.V)) return false;
                if (!this.Weighted.Equals(edge.Weighted)) return false;
                if (!this.GetHashCode().Equals(edge.GetHashCode())) return false;
                return true;
            }
        }
        /// <summary>
        /// Serves as a hash function for the type edge.
        /// The implementation of the GetHashCode method does not guarantee unique return values for different objects.
        /// The HasCode will be calculated with the GetHasCode functions from the vertex u and v. Transported edges have the same values.
        /// http://msdn.microsoft.com/en-us/library/system.object.gethashcode.aspx
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return Math.Abs(U.GetHashCode()) + Math.Abs(V.GetHashCode());
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Notify using String property name
        /// </summary>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region IEqualityComparer
        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/bb338049(v=vs.100).aspx
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(Edge e1, Edge e2)
        {
            //edge are equal
            if (e1.Equals(e2) && e2.Equals(e1)) return true;

            //edges are not equal but transposed (e1: v1->v2 e2: v2->v1 )
            if ((e1.Equals(e2) && e2.Equals(e1)).Equals(false) &&
                (e1.Equals(e2, true) && e2.Equals(e1, true)).Equals(true)) return true;

            //diffrent edges
            return false;
        }
        public int GetHashCode(Edge obj)
        {
            return obj.GetHashCode();
        }
        #endregion

    }
}
