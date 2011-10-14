using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Get.Graph
{
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

        public ObservableCollection<Vertex> Vertices
        {
            get
            {
                return _Vertices;
            }
        }
    }
}
