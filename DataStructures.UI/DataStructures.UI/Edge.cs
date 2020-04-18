using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DataStructures.UI
{
    [DataContract()]
    public class Edge : DataStructures.Edge, INotifyPropertyChanged
    {
        private IVertex _u;
        private IVertex _v;
        private int _weighted;

        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        public Edge(IVertex u, IVertex v) : base(u, v)
        {
        }
        /// <summary>
        /// Initializes a new instance of the Edge class.
        /// </summary>
        /// <param name="u">Vertex of the Edge</param>
        /// <param name="v">Vertex of the Edge</param>
        /// <param name="weighted">Sets the Weighted of the Edge</param>
        public Edge(IVertex u, IVertex v, int weighted) : base(u, v, weighted)
        {
        }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "U", Order = 2, IsRequired = true)]
        public override IVertex U { get { return _u; } set { _u = value; NotifyPropertyChanged(nameof(U)); } }
        /// <summary>
        /// Get or sets the Vertex of the Edge
        /// </summary>
        [DataMember(Name = "V", Order = 3, IsRequired = true)]
        public override IVertex V { get { return _v; } set { _v = value; NotifyPropertyChanged(nameof(V)); } }
        /// <summary>
        /// Gets or sets the Weighted of the Edge
        /// </summary>
        [DataMember(Name = "Weighted", IsRequired = true)]
        public override int Weighted { get { return _weighted; } set { _weighted = value; NotifyPropertyChanged(nameof(Weighted)); } }

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
