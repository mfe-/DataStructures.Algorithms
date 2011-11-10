using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Get.Model.Graph
{
    [XmlRoot("Graph")]
    public class Graph
    {
        public ObservableCollection<Vertex> _Vertices = new ObservableCollection<Vertex>();

        public Graph()
        {

        }
        public void addVertec(Vertex pVertice)
        {
            _Vertices.Add(pVertice);
        }
        [XmlArray("Vertices")]
        [XmlArrayItem("Vertex", typeof(Vertex))]
        public ObservableCollection<Vertex> Vertices
        {
            get
            {
                return _Vertices;
            }
        }
    }
}
