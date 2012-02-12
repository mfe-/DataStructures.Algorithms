using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace Get.Model.Graph
{
    public class Graph 
    {
        public ObservableCollection<Vertex> _Vertices = new ObservableCollection<Vertex>();

        public Graph()
        {
            _Vertices.LongCount();
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
