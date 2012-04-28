using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Get.Model.Graph
{
    [DataContract(Namespace = "http://schemas.get.com/Graph/")]
    public class Graph : INotifyPropertyChanged
    {
        [DataMember(Name = "Vertices")]
        public ObservableCollection<Vertex> _Vertices = new ObservableCollection<Vertex>();

        protected Vertex _StartVertex = null;
        protected Vertex _EndVertex = null;

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
