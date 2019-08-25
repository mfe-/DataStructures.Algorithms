using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics;
using System;

namespace DataStructures
{
    [DebuggerDisplay("Edge={Weighted},U={U},V={V}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge<TData> : IEdge<TData>
    {
        protected IVertex u;
        protected IVertex v;

        protected int _weighted;

        /// <summary>
        /// Initializes a new instance of the Edge class for using the IEqualityComparer on Distinct
        /// </summary>
        public Edge() { }

        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="pu">Vertex of the Edge</param>
        /// <param name="pv">Vertex of the Edge</param>
        public Edge(IVertex pu, IVertex pv)
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
        public Edge(IVertex pu, IVertex pv, int pweighted)
        {
            u = pu;
            v = pv;
            _weighted = pweighted;
        }
        public TData Value { get; set; }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "U", Order = 2, IsRequired = true)]
        public IVertex U { get { return u; } set { u = value; NotifyPropertyChanged("U"); } }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "V", Order = 3, IsRequired = true)]
        public IVertex V { get { return v; } set { v = value; NotifyPropertyChanged("V"); } }
        /// <summary>
        /// Gets or sets the Weighted of the Edge
        /// </summary>
        [DataMember(Name = "Weighted", IsRequired = true)]
        public int Weighted { get { return _weighted; } set { _weighted = value; NotifyPropertyChanged("Weighted"); } }
        
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
            if (!obj.GetType().Equals(typeof(Edge<TData>))) return false;

            //true if objA is the same instance as objB or if both are null; otherwise, false.
            if (Object.ReferenceEquals(this, obj)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(this, null) || Object.ReferenceEquals(obj, null)) return false;

            Edge<TData> edge = obj as Edge<TData>;

            return Equals(edge, false);
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

    }
    public interface IEdge : INotifyPropertyChanged
    {
        IVertex U { get; set; }
        IVertex V { get; set; }
        int Weighted { get; set; }
    }
    public interface IEdge<TData> : IEdge, IData<TData>
    {

    }
}
