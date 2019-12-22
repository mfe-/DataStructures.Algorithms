using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System;

namespace DataStructures
{
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

        public Graph()
        {
            _Vertices = new ObservableCollection<IVertex>();
        }

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
