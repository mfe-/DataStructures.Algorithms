using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Get.Graph
{
    public class Edge
    {
        protected Vertex u;
        protected Vertex v;

        protected int weighted;

        public Edge(Vertex pu, Vertex pv)
        {
            u = pu;
            v = pv;
        }

        public Edge(Vertex pu, Vertex pv, int pweighted)
        {
            u = pu;
            v = pv;
            weighted = pweighted;
        }
    }
}
