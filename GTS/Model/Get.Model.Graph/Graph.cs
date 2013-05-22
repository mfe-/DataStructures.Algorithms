using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Linq;
using Get.Common.Mathematics;
using System;
using System.Collections.Specialized;
using System.Collections.Generic;


namespace Get.Model.Graph
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/")]
    public class Graph : INotifyPropertyChanged
    {
        /// <summary>
        /// Store unconnected vertices
        /// </summary>
        [DataMember(Name = "Vertices")]
        protected ObservableCollection<Vertex> _Vertices = new ObservableCollection<Vertex>();

        protected Vertex _StartVertex = null;
        protected Vertex _EndVertex = null;

        public Graph()
        {
            _Vertices.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChanged);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the class should raise the CollectionChanged event and handle unconnected vertices.
        /// Disable this for better performance.
        /// </summary>
        protected bool _ManageUnconnectedVertices = true;
        public bool ManageUnconnectedVertices
        {
            get
            {
                return _ManageUnconnectedVertices;
            }
            set
            {
                _ManageUnconnectedVertices = value;
                if (_ManageUnconnectedVertices == false)
                {
                    _Vertices.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChanged);
                }
                else
                {
                    _Vertices.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChanged);
                }
                NotifyPropertyChanged("ManageUnconnectedVertices");
            }
        }
        /// <summary>
        /// Trys to determine if the vertex state changed to"unconnected" and needs to be safed in the list of the unconnected vertices. 
        /// If the state changes from unconnected to connected remove it from the "unconnected" vertices list.
        /// </summary>
        /// <param name="sender">Object which raised event</param>
        /// <param name="e">Provides some eventdata</param>
        protected virtual void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action.Equals(NotifyCollectionChangedAction.Add))
            {

                IList<object> items = e.NewItems.SyncRoot as IList<object>;
                object item = items.First();

                if (item.GetType().Equals(typeof(Edge)))
                {
                    Edge edge = item as Edge;

                    if (this.Vertices.Contains(edge.V))
                    {
                        //remove the connected vertex if its hold in the "unconnected" vertice list
                        if ((this.Vertices.Contains(edge.V) && this.Vertices.Contains(edge.U) && edge.V != StartVertex) || (StartVertex.DepthFirstSearch(edge.V).Count()!=0 && StartVertex.DepthFirstSearch(edge.V).Last().V.Equals(edge.V)))
                        {
                            this.Vertices.Remove(edge.V);
                        }

                    }
                    //regarding the new connection the unconnected vertices could be connected, so check them if they are connected and remove them.
                    for (int i = 0; i < Vertices.Count; i++)
                    {
                        IEnumerable<Edge> result = StartVertex.DepthFirstSearch(Vertices[i]);
                        if ((result.Count() != 0 && result.Last().V.Equals(Vertices[i])) && Vertices[i] != StartVertex)
                        {
                            _Vertices.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChanged);
                            _Vertices.RemoveAt(i);
                            _Vertices.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChanged);
                        }
                    }
                }
                if (item.GetType().Equals(typeof(Vertex)))
                {
                    Vertex vertex = item as Vertex;
                    if (StartVertex == null)
                    {
                        StartVertex = vertex;
                    }
                    vertex.Edges.CollectionChanged+=new NotifyCollectionChangedEventHandler(CollectionChanged);

                }
            }
            if (e.Action.Equals(NotifyCollectionChangedAction.Remove))
            {
                IList<object> items = e.OldItems.SyncRoot as IList<object>;
                object item = items.First();

                if (item.GetType().Equals(typeof(Edge)))
                {
                    //todo not implemented :code follows

                    //check if we can reach e.V
                    //Edge edge = item as Edge;
                    //IEnumerable<Edge> result  = StartVertex.DepthFirstSearch(edge.V);
                    //if(result.Count().Equals(0) || !(result.Count()!=0 && !result.Last().Equals(edge.V)))
                    //{
                    //    edge.V.Edges.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChanged);
                    //    _Vertices.Add(edge.V);
                    //    edge.V.Edges.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(CollectionChanged);
                    //}
                    //to here
                }
                if (item.GetType().Equals(typeof(Vertex)))
                {
                    Vertex vertex = item as Vertex;
                    //if (StartVertex == vertex)
                    //{
                    //    StartVertex = vertex;
                    //}
                    vertex.Edges.CollectionChanged -= new NotifyCollectionChangedEventHandler(CollectionChanged);

                }
            }
        }
        public void addVertex(Vertex pVertice)
        {
            //warum collection changed anmelden das bringt doch nichts?? nochmal überprüfen und dann entfernen wenn es keinen grund gibt das zu behalten
            //eigentlich würde das glaube ich sogar fehler produzieren, aber da die collectionchanged event sowieso jedesmal wieder abgemeldet wird kann nichts passieren
            //pVertice.Edges.CollectionChanged += new NotifyCollectionChangedEventHandler(CollectionChanged);
            _Vertices.Add(pVertice);
        }

        public ObservableCollection<Vertex> Vertices
        {
            get
            {
                return _Vertices;
            }
        }
        /// <summary>
        /// Gets or sets the start vertex of the graph
        /// </summary>
        [DataMember(Name = "StartVertex")]
        public Vertex StartVertex
        {
            get
            {
                return _StartVertex;
            }
            set
            {
                _StartVertex = value;
                NotifyPropertyChanged("StartVertex");
            }
        }
        /// <summary>
        /// Gets or sets the end vertex of the graph
        /// </summary>
        [DataMember(Name = "EndVertex")]
        public Vertex EndVertex
        {
            get
            {
                return _EndVertex;
            }
            set
            {
                _EndVertex = value;
                NotifyPropertyChanged("EndVertex");
            }
        }
        /// <summary>
        /// Gets or sets if the graph is directed
        /// </summary>
        [DataMember(Name = "Directed")]
        public bool Directed
        {
            get
            {
                //schauen ob alle vertex jeweils 2mal verbudnen sind also 1->2 und 2->1 nur dann ist es directed=false ansonsten directed=true
                //Schlichte ungerichtete Graphen haben daher eine symmetrische Adjazenzmatrix.
                //es muss daher von i nach j eine kantegeben und von j nach i, das entspricht aij=aji
                //also ist der graph genau dann ungerichtet wenn die matrix symmetrisch ist
                //http://en.wikipedia.org/wiki/Transpose
                //a transportieren also a^t = a symmetrisch
                int[][] matrix = this.AdjacencyList();
                int[][] matrixT = new int[matrix.Length][];
                for (int i = 0; i < matrix.Length; i++)
                {
                    matrixT[i] = new int[matrix.Length];
                }

                //transportieren
                for (int i = 0; i < matrix.Length; i++)
                    for (int j = 0; j < matrix[i].Length; j++)
                        matrixT[j][i] = matrix[i][j];

                //check if result1 = matrixT1 --> symmetry --> v1->v2 & v1<-v2 = undirected
                for (int i = 0; i < matrix.Length; i++)
                    for (int j = 0; j < matrix[i].Length; j++)
                    {
                        if (matrix[i][j] != matrixT[i][j]) return true;
                    }

                return false;
            }
            set
            {
                if (this.Vertices == null) this._Vertices = new ObservableCollection<Vertex>();
                if (value.Equals(false))
                {
                    //Interpreatoin einer ungerichteten Kante als Paar gerichtter Kanten v1-->v2 & v1<--v2 = v1---v2
                    //get all edges and check if there exists a couple of edge otherwise add the missing one
                    foreach (Vertex v in this.Depth_First_Traversal().Sort().Distinct<Vertex>())
                        foreach (Edge e in v.Edges)
                        {
                            if (e.V.Edges.Where(a => a.V.Equals(v)).Count().Equals(0))
                            {
                                e.V.addEdge(v,e.Weighted);
                            }

                        }

                }
                else
                {
                    //wenn directed auf true gesetzt wird, alle doppelten edges löschen (eigentlich würde es reichen eine einzige kante zu entfernen)
                    foreach (Vertex v in this.Depth_First_Traversal().Sort().Distinct<Vertex>())
                        foreach (Edge e in v.Edges)
                        {
                            Edge edgeback = e.V.Edges.Where(a => a.V.Equals(v)).First<Edge>();
                            e.V.Edges.Remove(edgeback);

                        }
                }
                NotifyPropertyChanged("Directed");
                
            }
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
    public class DirectedException : Exception
    {
        protected const String Directedmessage = "Graph must be directed";
        protected const String Undirectedmessage = "Graph must be undirected";

        public DirectedException(bool pDirected)
            : base(pDirected==true ? Directedmessage : Undirectedmessage)
        {

        }
    }

}
