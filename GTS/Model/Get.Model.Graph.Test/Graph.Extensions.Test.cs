using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Get.Model.Graph.Test
{
    [TestClass]
    public class UnitTest1
    {

        public Graph g;

        [TestInitialize]
        public void Initialize()
        {
            this.g = new Graph();
            //g.Load(Environment.CurrentDirectory + "\\dijkstra.xml");
        }
        [TestMethod]
        public void DepthFirstSearchTest()
        {
            Graph g = new Graph();
            Vertex v1 = new Vertex() { Weighted = 1 };
            Vertex v2 = new Vertex() { Weighted = 2 }; ;
            Vertex v3 = new Vertex() { Weighted = 3 }; ;
            Vertex v4 = new Vertex() { Weighted = 4 }; ;
            Vertex v5 = new Vertex() { Weighted = 5 }; ;
            Vertex v6 = new Vertex() { Weighted = 6 }; ;
            Vertex v7 = new Vertex() { Weighted = 7 }; ;

            g.addVertex(v1);

            v1.addEdge(v2,2);
            v1.addEdge(v3,5);
            v1.addEdge(v4,3);

            v2.addEdge(v3,4);
            v2.addEdge(v5,6);

            v3.addEdge(v5,4);
            v3.addEdge(v6,1);
            v3.addEdge(v4,1);

            v4.addEdge(v6,3);

            v5.addEdge(v6,2);

            v6.addEdge(v7,5);

            v7.addEdge(v5,2);


            var result = v1.DepthFirstSearch(v6);

            //directed testen
            //g.Directed

        }
        [TestMethod]
        public void dftTest()
        {

            //hier die extension dft aufrufen und nacher schauen ob alle vertexes besucht wurden

        }
    }
}
