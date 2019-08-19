using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Get.Model.Graph.Test
{
    [TestClass]
    public class GraphTest
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
        public void DirectedTest()
        {



            //Directed Propertie will not be evaluated - so this test is obsolet



            //Assert.AreEqual(g.Directed, true);

            //TestContext.WriteLine("g.Directed: " + g.Directed + " Graph.Edges: " + g.Depth_First_Traversal().Sort().Distinct<Vertex>().SelectMany(a => a.Edges).Distinct<Edge>().Count());

            ////create undirected graph with doppeltenkanten z.b aus v1-v2 wird mit v1->v2 und v1<-v2 dargestellt 
            //foreach (Vertex v in g.Depth_First_Traversal().Sort().Distinct<Vertex>())
            //    foreach (Edge e in v.Edges)
            //    {
            //        if (e.V.Edges.Where(a => a.V.Equals(v)).Count().Equals(0))
            //        {
            //            e.V.addEdge(v);
            //        }

            //    }

            //Assert.AreEqual(g.Directed, false);

            //TestContext.WriteLine("g.Directed: " + g.Directed + " Graph.Edges: " + g.Depth_First_Traversal().Sort().Distinct<Vertex>().SelectMany(a => a.Edges).Distinct<Edge>().Count());


            //g.Directed = true;

            //TestContext.WriteLine("g.Directed: " + g.Directed + " Graph.Edges: " + g.Depth_First_Traversal().Sort().Distinct<Vertex>().SelectMany(a => a.Edges).Distinct<Edge>().Count());
            //Assert.IsTrue(g.Directed);

        }
        
        //[TestMethod]
        //public void CollectionChangedTest()
        //{
        //    v1 = new Vertex(1);
        //    v2 = new Vertex(2);
        //    v3 = new Vertex(3);
        //    v4 = new Vertex(4);
        //    v5 = new Vertex(5);
        //    v6 = new Vertex(6);
        //    v7 = new Vertex(7);

        //    g = new Graph();
        //    //add edges
        //    g.addVertex(v1);
        //    g.addVertex(v2);
        //    g.addVertex(v3);
        //    g.addVertex(v4);
        //    g.addVertex(v5);
        //    g.addVertex(v6);
        //    g.addVertex(v7);

        //    v3.addEdge(v6, 0, true);
        //    //v6 will be connected with v3 ... so v6 is not required to hold in list
        //    Assert.AreEqual(g.Vertices.Contains(v6), false);
        //    Assert.AreEqual(g.Vertices.Count, 6);

        //    v3.addEdge(v4, 0, true);
        //    Assert.AreEqual(g.Vertices.Contains(v4), false);
        //    Assert.AreEqual(g.Vertices.Count, 5);

        //    v1.addEdge(v2, 0, true);
        //    Assert.AreEqual(g.Vertices.Contains(v2), false);
        //    v5.addEdge(v6, 0, true);
        //    Assert.AreEqual(g.Vertices.Contains(v6), false);
        //    v5.addEdge(v7, 0, true);
        //    Assert.AreEqual(g.Vertices.Contains(v7), false);
        //    v1.addEdge(v4, 0, true);
        //    Assert.AreEqual(g.Vertices.Contains(v4), false);
        //    v4.addEdge(v6, 0, true);

        //    Assert.AreEqual(g.Vertices.Count, 1);

        //    //remove all edges
        //    v6.Edges.RemoveAt(2);
        //    v4.Edges.RemoveAt(2);

        //    Assert.AreEqual(g.StartVertex.DepthFirstSearch(v6).Last().V.Equals(v6), true);
        //    Assert.AreEqual(g.StartVertex.DepthFirstSearch(v4).Last().V.Equals(v4), true);

        //    //todo not implemented

        //    g = new Graph();

        //    v1 = new Vertex(1);
        //    v2 = new Vertex(2);
        //    v3 = new Vertex(3);
        //    v4 = new Vertex(4);
        //    v5 = new Vertex(5);
        //    v6 = new Vertex(6);
        //    v7 = new Vertex(7);

        //    v1.addEdge(v2, 0, true);
        //    v2.addEdge(v3, 0, true);

        //    v4.addEdge(v5, 0, true);
        //    v5.addEdge(v6, 0, true);

        //    g.addVertex(v1);
        //    Assert.AreEqual(g.Vertices.Count, 1);

        //    g.addVertex(v4);
        //    Assert.AreEqual(g.Vertices.Count, 2);

        //    v3.addEdge(v4,0,true);
        //    Assert.AreEqual(g.Vertices.Count, 1);
        //}

        public TestContext TestContext { get; set; }

    }
}

