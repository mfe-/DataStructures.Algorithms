using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Algorithms.Graph;
using Xunit;

namespace DataStructures.Test
{
    public class GraphExensionsTest
    {

        public Graph g;
        IVertex v1 = new Vertex<object>() { Weighted = 1 };
        IVertex v2 = new Vertex<object>() { Weighted = 2 };
        IVertex v3 = new Vertex<object>() { Weighted = 3 };
        IVertex v4 = new Vertex<object>() { Weighted = 4 };
        IVertex v5 = new Vertex<object>() { Weighted = 5 };
        IVertex v6 = new Vertex<object>() { Weighted = 6 };
        IVertex v7 = new Vertex<object>() { Weighted = 7 };

        public GraphExensionsTest()
        {
            Graph g = new Graph();

            g.AddVertex(v1);

            v1.AddEdge(v2, 2);
            v1.AddEdge(v3, 5);
            v1.AddEdge(v4, 3);

            v2.AddEdge(v3, 4);
            v2.AddEdge(v5, 6);

            v3.AddEdge(v5, 4);
            v3.AddEdge(v6, 1);
            v3.AddEdge(v4, 1);

            v4.AddEdge(v6, 3);

            v5.AddEdge(v6, 2);

            v6.AddEdge(v7, 5);

            v7.AddEdge(v5, 2);

            this.g = g;
        }

        [Fact]
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
                    Assert.Equal(matrix[i][j], result[i][j]);

        }
        [Fact]
        public void DepthFirstSearch_should_find_circles_in_undirected_graph()
        {
            XElement xmlElement = XElement.Parse(EmbeddedResourceLoader.GetFileContents("dijkstra.xml"));
            xmlElement = xmlElement.Elements().FirstOrDefault(a => a.Name.LocalName.Equals("Graph"));

            Graph g = GraphExtensions.Load(xmlElement);

            //undirected graph required
            Assert.False(g.IsDirected());

            g.Directed = true;
            //v1
            v1 = null;
            v2 = null;
            v3 = null;
            v4 = null;
            v5 = null;
            v6 = null;
            v1 = g.Start;
            v2 = v1.Edges.FirstOrDefault(a => a.V.Weighted == 2).V;

            //there is a directed circle from 2-...->2
            var resultv2 = v2.DepthFirstSearch(v2);
            PrintEdges(resultv2);
            Assert.Equal(resultv2.Last().V, v2);
            //there is a directed circle from 3-...->3
            v3 = v2.Edges.FirstOrDefault(a => a.V.Weighted == 3).V;
            var resultv3 = v3.DepthFirstSearch(v3);
            PrintEdges(resultv3);
            Assert.Equal(resultv3.Last().V, v3);

            v6 = v3.Edges.FirstOrDefault(a => a.V.Weighted == 6).V;
            var resultv6 = v6.DepthFirstSearch(v6);
            PrintEdges(resultv6);
            Assert.Equal(resultv6.Last().V, v6);

        }
        [Fact]
        public void DepthFirstSearch_should_find_paths_on_directed_and_undirected_graph()
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
            Assert.Equal(resultv1.Count(), 4);
            Assert.Equal(resultv1.Last().V, v6);

            //look for 1
            //6->7->5->6
            var resultv6 = v6.DepthFirstSearch(v1);
            Assert.Equal(resultv6.Count(), 2);
            //when moving from 5->6 , 6 will not be added to the edge list, 
            //because the only left edge is not the goal 
            Assert.Equal(resultv6.Last().V, v5);

            //circule detection
            Vertex<object> a = new Vertex<object>(1);
            Vertex<object> b = new Vertex<object>(2);

            //2 undirected connected Vertices
            a.AddEdge(b);
            b.AddEdge(a);

            var resultva = b.DepthFirstSearch(b);
            Assert.NotEqual(resultva.First().U, resultva.Last().V);

            //3 undirected connected vertices with circle
            Vertex<object> c = new Vertex<object>(3);
            //connect a with c
            a.AddEdge(c);
            c.AddEdge(a);
            //connect b with c
            b.AddEdge(c);
            c.AddEdge(b);
            var resultvc = c.DepthFirstSearch(c);
            Assert.Equal(resultvc.Last().V, c);

            //there is no directed circle from 2-...->2
            var resultv2 = v2.DepthFirstSearch(v2);
            Assert.NotEqual(resultv2.Last().V, v2);

            //there is no directed circle from 3-...->3
            var resultv3 = v3.DepthFirstSearch(v3);
            Assert.NotEqual(resultv3.Last().V, v3);

            resultv6 = v6.DepthFirstSearch(v6);
            Assert.Equal(resultv6.Last().V, v6);

            //circule detection in paths
            a = new Vertex<object>(1);
            b = new Vertex<object>(2);
            c = new Vertex<object>(3);

            //create undirected pah
            a.AddEdge(b);
            b.AddEdge(a);
            b.AddEdge(c);
            c.AddEdge(b);
            var resultp = a.DepthFirstSearch(a);
            Assert.NotEqual(resultp.First(), resultp.Last());

            //example which occours in kruskal
            a = new Vertex<object>(3);
            b = new Vertex<object>(6);
            c = new Vertex<object>(4);

            a.AddEdge(b);
            b.AddEdge(a);

            b.AddEdge(c);
            c.AddEdge(b);

            var result34 = a.DepthFirstSearch(a);
            Assert.NotEqual(result34.First(), result34.Last());

            //example which occours in kruskal
            v1 = new Vertex<object>(1);
            v2 = new Vertex<object>(2);
            v3 = new Vertex<object>(3);
            v4 = new Vertex<object>(4);
            v5 = new Vertex<object>(5);
            v6 = new Vertex<object>(6);
            v7 = new Vertex<object>(7);

            v3.AddEdge(v6, 0, false);
            v3.AddEdge(v4, 0, false);
            v1.AddEdge(v2, 0, false);
            v5.AddEdge(v6, 0, false);
            v5.AddEdge(v7, 0, false);
            v1.AddEdge(v4, 0, false);
            v4.AddEdge(v6, 0, false);

            var circule634 = v6.DepthFirstSearch(v6);
            Assert.Equal(circule634.Last().V, v6);

        }

        [Fact]
        public void KruskalTest()
        {
            //check if exception will be thrown
            try
            {
                g.Kruskal_DepthFirstSearch();
            }
            catch (DirectedException de)
            {
                g.Directed = false;
            }

            int a = g.Kruskal_DepthFirstSearch().Depth_First_Traversal().SelectMany(z => z.Edges).Distinct(new EdgeExtensions.EdgeComparer()).Sum(b => b.Weighted);
            Assert.Equal(a, 11);
        }
        [Fact]
        public void Breadth_First_Search()
        {
            g.Start.Breadth_First_Search(v7);

        }
        [Fact]
        public void Deph_First_Search()
        {
            List<IVertex> result = g.Start.Deph_First_Search().ToList();

            List<IVertex> result2 = g.Depth_First_Traversal().ToList();

            foreach (IVertex v in result2)
            {
                Assert.True(result.Contains(v));
            }
        }
        public void PrintEdges(IEnumerable<IEdge> edges)
        {
            Console.WriteLine($"=====================");
            foreach (var edge in edges)
            {
                Console.WriteLine($"{edge.U} -> {edge.V}");
            }
        }


    }
}
