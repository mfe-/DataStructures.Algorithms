using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Diagnostics;

namespace DataStructures.UI
{
    [DebuggerDisplay("Vertex={Weighted},GUID={_Guid}")]
    [DataContract()]
    public class Vertex : DataStructures.Vertex, INotifyPropertyChanged
    {
        private ObservableCollection<IEdge> _Edges;
        private int _weighted;
        /// <summary>
        /// Initializes a new instance of the Vertex class.
        /// </summary>
        public Vertex() : base()
        {
            _Edges = new ObservableCollection<IEdge>();
        }
        public Vertex(int weighted) : this()
        {
            _weighted = weighted;
        }

        /// <summary>
        /// Gets or sets the Weighted of the vertex
        /// </summary>
        public override int Weighted { get { return _weighted; } set { _weighted = value; NotifyPropertyChanged(nameof(Weighted)); } }

        /// <summary>
        /// Gets or sets the list of edges which connects the vertex neighbours
        /// </summary>
        public override ICollection<IEdge> Edges
        {
            get { return _Edges; }
            protected set { _Edges = new ObservableCollection<IEdge>(value); NotifyPropertyChanged(nameof(Edges)); }
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
