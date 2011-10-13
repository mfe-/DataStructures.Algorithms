using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Get.Graph
{
    public class Graph
    {
        public ObservableCollection<Vertex> Vertices = new ObservableCollection<Vertex>();

        public Graph()
        {

        }
        public void addVertec(Vertex pVertice)
        {
            Vertices.Add(pVertice);
        }
    }
}
