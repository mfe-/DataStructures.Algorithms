using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System;

namespace DataStructures
{
    /// <summary>
    /// Graph, a structure made of vertices and edges
    /// </summary>
    [DataContract(Namespace = "http://schemas.get.com/Graph/")]
    public class Graph : INotifyPropertyChanged
    {
        private bool directed = false;
        /// <summary>
        /// Store unconnected vertices
        /// </summary>
        [DataMember(Name = nameof(Vertices))]
        private ObservableCollection<IVertex> _Vertices = new ObservableCollection<IVertex>();

        private IVertex? _Start = null;
        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph()
        {
            _Vertices = new ObservableCollection<IVertex>();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        /// <param name="directed">Sets true if the graph is directed.</param>
        public Graph(bool directed)
        {
            this.directed = directed;
        }
        /// <summary>
        /// Saves unconnected vertices. If you connect an unconnected vertex you have to remove it from the list!
        /// </summary>
        public ObservableCollection<IVertex> Vertices
        {
            get
            {
                return _Vertices;
            }
        }
        /// <summary>
        /// Adds a vertex to the current graph.
        /// </summary>
        /// <param name="pVertice">The vertex to add</param>
        public void AddVertex(IVertex pVertice)
        {
            _Vertices.Add(pVertice);

            if (Start == null)
            {
                Start = pVertice;
            }
        }
        /// <summary>
        /// Gets or sets the start vertex of the graph
        /// </summary>
        [DataMember(Name = "StartVertex")]
        public IVertex? Start
        {
            get
            {
                return _Start;
            }
            set
            {
                _Start = value;
                if (_Start != null && Vertices != null && Vertices.Count == 0)
                {
                    Vertices.Add(_Start);
                }
                NotifyPropertyChanged(nameof(Start));
            }
        }
        private Func<IVertex>? _CreateVertexFunc;
        /// <summary>
        /// Get or sets the function for creating vertices
        /// </summary>
        public Func<IVertex>? CreateVertexFunc
        {
            get { return _CreateVertexFunc; }
            set
            {
                _CreateVertexFunc = value;
                NotifyPropertyChanged(nameof(CreateVertexFunc));
            }
        }
        /// <summary>
        /// Gets or sets whether the graph is directed or undirected.
        /// This value does not get computed and has no effect on the graph when set.
        /// </summary>
        [DataMember(Name = nameof(Directed))]
        public bool Directed { get { return directed; } set { directed = value; NotifyPropertyChanged(nameof(Directed)); } }

        #region INotifyPropertyChanged
        /// <summary>
        /// Event raised when a property is changed on the <see cref="Graph"/> component.
        /// </summary>
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
