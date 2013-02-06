using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Get.Model.Graph.Test
{
    [TestClass]
    public class GraphExensionsTest
    {

        public Graph g;
        Vertex v1 = new Vertex() { Weighted = 1 };
        Vertex v2 = new Vertex() { Weighted = 2 };
        Vertex v3 = new Vertex() { Weighted = 3 };
        Vertex v4 = new Vertex() { Weighted = 4 };
        Vertex v5 = new Vertex() { Weighted = 5 };
        Vertex v6 = new Vertex() { Weighted = 6 };
        Vertex v7 = new Vertex() { Weighted = 7 };

        [TestInitialize]
        public void Initialize()
        {
            Graph g = new Graph();

            g.addVertex(v1);

            v1.addEdge(v2, 2);
            v1.addEdge(v3, 5);
            v1.addEdge(v4, 3);

            v2.addEdge(v3, 4);
            v2.addEdge(v5, 6);

            v3.addEdge(v5, 4);
            v3.addEdge(v6, 1);
            v3.addEdge(v4, 1);

            v4.addEdge(v6, 3);

            v5.addEdge(v6, 2);

            v6.addEdge(v7, 5);

            v7.addEdge(v5, 2);

            this.g = g;
        }

        [TestMethod]
        public void AdjacencyListTest()
        {

            int[][] matrix = g.AdjacencyList();

            int[][] result = {new int[]{0,0,0,0,0,0,0},
                              new int[]{1,0,0,0,0,0,0},
                              new int[]{1,1,0,0,0,0,0},
                              new int[]{1,0,1,0,0,0,0},
                              new int[]{0,1,1,0,0,0,1},
                              new int[]{0,0,1,1,1,0,0},
                              new int[]{0,0,0,0,0,1,0}
                             };

            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    Assert.AreEqual(matrix[i][j], result[i][j]);

        }

        [TestMethod]
        public void DepthFirstSearchTest()
        {

            //directed testen
            if (g.Directed == true)
            {
                g.Directed = false;
            }

            //1-6
            var resultv1 = v1.DepthFirstSearch(v6);
            //some possible ways
            //1->3->6, 1->3->5->6,1->2->3->5->6,1->4->6
            //because of so much possible solutions we equal them like that way
            Assert.AreEqual(resultv1.Count(), 4);
            Assert.AreEqual(resultv1.Last().V, v6);


            var resultv6 = v6.DepthFirstSearch(v1);
            Assert.AreEqual(resultv6.Count(), 6);
            Assert.AreEqual(resultv6.Last().V, v1);

            //circule detection
            var resultv2 = v2.DepthFirstSearch(v2);
            Assert.AreEqual(resultv2.Last().V, v2);

            var resultv3 = v3.DepthFirstSearch(v3);
            Assert.AreEqual(resultv3.Last().V, v3);

            resultv6 = v6.DepthFirstSearch(v6);
            Assert.AreEqual(resultv6.Last().V, v6);
        }

        [TestMethod]
        public void KruskalTest()
        {
            //check if exception will be thrown
            try
            {
                g.Kruskal();
            }
            catch (DirectedException de)
            {
                g.Directed = false;
            }

            g.Kruskal();
        }

        public TestContext TestContext { get; set; }


    }
}
