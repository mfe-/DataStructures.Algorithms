using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;
using Get.Common.Mathematics;


namespace Get.Model.Graph
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/")]
    public class Graph : INotifyPropertyChanged
    {
        [DataMember(Name = "Vertices")]
        public ObservableCollection<Vertex> _Vertices = new ObservableCollection<Vertex>();

        protected Vertex _StartVertex = null;
        protected Vertex _EndVertex = null;
        protected bool _directed = false;

        public Graph()
        {
        }
        public void addVertex(Vertex pVertice)
        {
            if (StartVertex == null)
                StartVertex = pVertice;

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
                
                //Handschlaglemma: In einem schlichten ungerichteten Graphen G gilt: sum(d(v))_(v e V(G))=2|E(g)|

                //todo schauen ob alle vertex jeweils 2mal verbudnen sind also 1->2 und 2->1 nur dann ist es directed=false!
                //ansonsten directed=true
                Matrix m = new Matrix(this.AdjacencyList());

                
                return _directed;
            }
            set
            {
                if (value.Equals(false))
                {
                    //Interpreatoin einer ungerichteten Kante als Paar gerichtter Kanten v1-->v2 & v1<--v2 = v1---v2
                    //get all edges and check if there exists a couple of edge otherwise add the missing one


                }
                else
                {
                    //todo wenn directed auf false gesetzt wird überall fehlende edges hinzufügen!
                }
                _directed = value;
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

}
