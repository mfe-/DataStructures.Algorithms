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
            //schauen ob es wenn es von i nach j eine kante gibt es auch von j nach i eine gibt, das entspricht grade aij=aji
            //also ist der graph genau dann ungerichtet wenn die matrix symmetrisch ist

            //http://en.wikipedia.org/wiki/Transpose
            //a transportieren also a^t = a symmetrisch

            int[][] matrix = g.AdjacencyList();
            int[][] matrixT = new int[matrix.Length][];

            for (int i = 0; i < matrix.Length; i++)
            {
                matrixT[i] = new int[matrix.Length];
            }

            //transportieren
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    matrixT[j][i] = matrix[i][j];

            int[][] result = {new int[] {0,1,1,1,0,0,0},
                              new int[] {0,0,1,0,1,0,0},
                              new int[] {0,0,0,1,1,1,0},
                              new int[] {0,0,0,0,0,1,0},
                              new int[] {0,0,0,0,0,1,0},
                              new int[] {0,0,0,0,0,0,1},
                              new int[] {0,0,0,0,1,0,0}
                             };

            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrixT[i].Length; j++)
                    Assert.AreEqual(matrixT[i][j], result[i][j]);

            //create undirected graph with doppeltenkanten z.b aus v1-v2 wird mit v1->v2 und v1<-v2 dargestellt 
            foreach (Vertex v in g.Depth_First_Traversal().Sort().Distinct<Vertex>())
                foreach (Edge e in v.Edges)
                {
                    if (e.V.Edges.Where(a => a.V.Equals(v)).Count().Equals(0))
                    {
                        e.V.addEdge(v);
                    }

                }

            matrix = g.AdjacencyList();

            int[][] result1 = {new int[] {0,1,1,1,0,0,0},
                               new int[] {1,0,1,0,1,0,0},
                               new int[] {1,1,0,1,1,1,0},
                               new int[] {1,0,1,0,0,1,0},
                               new int[] {0,1,1,0,0,1,1},
                               new int[] {0,0,1,1,1,0,1},
                               new int[] {0,0,0,0,1,1,0}
                             };

            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    Assert.AreEqual(matrix[i][j], result1[i][j]);

            int[][] matrixT1 = new int[matrix.Length][];

            for (int i = 0; i < matrix.Length; i++)
            {
                matrixT1[i] = new int[matrix.Length];
            }

            //transportieren
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    matrixT1[j][i] = matrix[i][j];

            //check if result1 = matrixT1 --> symmetry --> v1->v2 & v1<-v2 = undirected
            for (int i = 0; i < matrix.Length; i++)
                for (int j = 0; j < matrix[i].Length; j++)
                    Assert.AreEqual(matrixT1[i][j], result1[i][j]);

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
