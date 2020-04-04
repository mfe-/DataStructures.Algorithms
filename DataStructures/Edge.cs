using System.ComponentModel;
using System.Runtime.Serialization;
using System.Diagnostics;
using System;
using System.Linq;

namespace DataStructures
{
    [DebuggerDisplay("U={U}->V={V},Edge={Weighted}")]
    [DataContract(Namespace = "http://schemas.get.com/Graph/Edges")]
    public class Edge : IEdge
    {
        private IVertex _u;
        private IVertex _v;
        private int _weighted;

        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        public Edge(IVertex u, IVertex v)
        {
            _u = u;
            _v = v;
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        /// <param name="weighted">Sets the Weighted of the Edge</param>
        public Edge(IVertex u, IVertex v, int weighted) : this(u, v)
        {
            _weighted = weighted;
        }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "U", Order = 2, IsRequired = true)]
        public IVertex U { get { return _u; } set { _u = value; NotifyPropertyChanged(nameof(U)); } }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "V", Order = 3, IsRequired = true)]
        public IVertex V { get { return _v; } set { _v = value; NotifyPropertyChanged(nameof(V)); } }
        /// <summary>
        /// Gets or sets the Weighted of the Edge
        /// </summary>
        [DataMember(Name = "Weighted", IsRequired = true)]
        public int Weighted { get { return _weighted; } set { _weighted = value; NotifyPropertyChanged(nameof(Weighted)); } }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"{U} -> {V}";
        }

        public override bool Equals(object obj)
        {
            if (!(obj is IEdge)) return false;
            IEdge edge = (IEdge)obj;
            //return (edge)?.GetHashCode() == this.GetHashCode();
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

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Notify using String property name
        /// </summary>
        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
