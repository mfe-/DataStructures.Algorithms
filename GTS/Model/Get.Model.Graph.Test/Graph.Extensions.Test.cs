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
            Graph g = new Graph();
            Vertex v1 = new Vertex() { Weighted = 1 };
            Vertex v2 = new Vertex() { Weighted = 2 }; ;
            Vertex v3 = new Vertex() { Weighted = 3 }; ;
            Vertex v4 = new Vertex() { Weighted = 4 }; ;
            Vertex v5 = new Vertex() { Weighted = 5 }; ;
            Vertex v6 = new Vertex() { Weighted = 6 }; ;
            Vertex v7 = new Vertex() { Weighted = 7 }; ;

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

         for( int i = 0; i < matrix.Length; i++ ) 
				for( int j = 0; j < matrix[i].Length; j++ ) 
					Assert.AreEqual(matrix[i][j], result[i][j]);

        }
        [TestMethod]
        public void DirectedTest()
        {
            Assert.AreEqual(g.Directed, true);

            TestContext.WriteLine("g.Directed: " + g.Directed + " Graph.Edges: " + g.Depth_First_Traversal().Sort().Distinct<Vertex>().SelectMany(a => a.Edges).Distinct<Edge>().Count());

            //create undirected graph with doppeltenkanten z.b aus v1-v2 wird mit v1->v2 und v1<-v2 dargestellt 
            foreach (Vertex v in g.Depth_First_Traversal().Sort().Distinct<Vertex>())
                foreach (Edge e in v.Edges)
                {
                    if (e.V.Edges.Where(a => a.V.Equals(v)).Count().Equals(0))
                    {
                        e.V.addEdge(v);
                    }

                }

            Assert.AreEqual(g.Directed, false);

            TestContext.WriteLine("g.Directed: " + g.Directed + " Graph.Edges: " + g.Depth_First_Traversal().Sort().Distinct<Vertex>().SelectMany(a => a.Edges).Distinct<Edge>().Count());


            g.Directed = true;

            TestContext.WriteLine("g.Directed: " + g.Directed + " Graph.Edges: " + g.Depth_First_Traversal().Sort().Distinct<Vertex>().SelectMany(a => a.Edges).Distinct<Edge>().Count());
            Assert.IsTrue(g.Directed);




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

            var result = v1.DepthFirstSearch(v6);

            //directed testen
            //g.Directed

        }

        public TestContext TestContext { get;set; }
        [TestMethod]
        public void dftTest()
        {

            //hier die extension dft aufrufen und nacher schauen ob alle vertexes besucht wurden

        }
    }
}
