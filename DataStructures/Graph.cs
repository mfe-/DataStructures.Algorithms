using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System;


namespace DataStructures
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/")]
    public class Graph : INotifyPropertyChanged
    {
        protected bool directed = false;
        /// <summary>
        /// Store unconnected vertices
        /// </summary>
        [DataMember(Name = "Vertices")]
        protected ObservableCollection<IVertex> _Vertices = new ObservableCollection<IVertex>();

        protected IVertex? _Start = null;

        public Graph() { }

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
                NotifyPropertyChanged("StartVertex");
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
        /// Gets or sets if the graph is directed
        /// </summary>
        //[DataMember(Name = "Directed")]
        //public bool Directed
        //{
        //    get
        //    {
        //        //schauen ob alle vertex jeweils 2mal verbudnen sind also 1->2 und 2->1 nur dann ist es directed=false ansonsten directed=true
        //        //Schlichte ungerichtete Graphen haben daher eine symmetrische Adjazenzmatrix.
        //        //es muss daher von i nach j eine kantegeben und von j nach i, das entspricht aij=aji
        //        //also ist der graph genau dann ungerichtet wenn die matrix symmetrisch ist
        //        //http://en.wikipedia.org/wiki/Transpose
        //        //a transportieren also a^t = a symmetrisch
        //        int[][] matrix = this.AdjacencyList();
        //        int[][] matrixT = new int[matrix.Length][];
        //        for (int i = 0; i < matrix.Length; i++)
        //        {
        //            matrixT[i] = new int[matrix.Length];
        //        }

        //        //transportieren
        //        for (int i = 0; i < matrix.Length; i++)
        //            for (int j = 0; j < matrix[i].Length; j++)
        //                matrixT[j][i] = matrix[i][j];

        //        //check if result1 = matrixT1 --> symmetry --> v1->v2 & v1<-v2 = undirected
        //        for (int i = 0; i < matrix.Length; i++)
        //            for (int j = 0; j < matrix[i].Length; j++)
        //            {
        //                if (matrix[i][j] != matrixT[i][j]) return true;
        //            }

        //        return false;
        //    }
        //    set
        //    {
        //        if (this.Vertices == null) this._Vertices = new ObservableCollection<Vertex>();
        //        if (value.Equals(false))
        //        {
        //            //Interpreatoin einer ungerichteten Kante als Paar gerichtter Kanten v1-->v2 & v1<--v2 = v1---v2
        //            //get all edges and check if there exists a couple of edge otherwise add the missing one
        //            foreach (Vertex v in this.Depth_First_Traversal().Sort().Distinct<Vertex>())
        //                foreach (Edge e in v.Edges)
        //                {
        //                    if (e.V.Edges.Where(a => a.V.Equals(v)).Count().Equals(0))
        //                    {
        //                        e.V.addEdge(v, e.Weighted);
        //                    }

        //                }

        //        }
        //        else
        //        {
        //            //wenn directed auf true gesetzt wird, alle doppelten edges löschen (eigentlich würde es reichen eine einzige kante zu entfernen)
        //            foreach (Vertex v in this.Depth_First_Traversal().Sort().Distinct<Vertex>())
        //                foreach (Edge e in v.Edges)
        //                {
        //                    Edge edgeback = e.V.Edges.Where(a => a.V.Equals(v)).First<Edge>();
        //                    e.V.Edges.Remove(edgeback);

        //                }
        //        }
        //        NotifyPropertyChanged("Directed");

        //    }


        /// <summary>
        /// Gets or sets the start vertex of the graph
        /// </summary>
        [DataMember(Name = "Directed")]
        public bool Directed { get { return directed; } set { directed = value; NotifyPropertyChanged("Directed"); } }



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
    public class DirectedException : Exception
    {
        protected const String Directedmessage = "Graph must be directed";
        protected const String Undirectedmessage = "Graph must be undirected";

        public DirectedException(bool directed)
            : base(directed ? Directedmessage : Undirectedmessage)
        {

        }
    }

}
