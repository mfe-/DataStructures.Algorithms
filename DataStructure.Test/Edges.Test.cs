using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataStructures.Test
{
    [TestClass]
    public class EdgesTest
    {
        Graph g;
        IVertex v1;
        IVertex v2;
        IVertex v3;
        IVertex v4;
        IVertex v5;
        IVertex v6;
        IVertex v7;

        [TestInitialize]
        public void Initialize()
        {
            v1 = new Vertex<object>() { Weighted = 1 };
            v2 = new Vertex<object>() { Weighted = 2 };
            v3 = new Vertex<object>() { Weighted = 3 };
            v4 = new Vertex<object>() { Weighted = 4 };
            v5 = new Vertex<object>() { Weighted = 5 };
            v6 = new Vertex<object>() { Weighted = 6 };
            v7 = new Vertex<object>() { Weighted = 7 };
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
        [TestMethod]
        public void GetHashCodeTest()
        {
            //hascode of transported edges must be the same
            IEdge e1 = g.Vertices.First().Edges.First();
            //create a transported edge
            e1.V.AddEdge(e1.U, e1.Weighted);

            IEdge e2 = e1.V.Edges.Last();

            //transported edge needs same hascode
            Assert.AreEqual(e1.GetHashCode(), e2.GetHashCode());

            //random edge - diffrent hascode
            Assert.AreNotEqual(e1.GetHashCode(), v4.Edges.First().GetHashCode());
        }
        [TestMethod]
        public void EqualsTest()
        {
            Vertex<object> v1 = new Vertex<object>(1);
            Vertex<object> v2 = new Vertex<object>(2);

            v1.AddEdge(v2);
            v2.AddEdge(v1);

            IEdge e1 = v1.Edges.First();
            IEdge e2 = v2.Edges.First();

            //e1=e1
            Assert.IsTrue(e1.Equals(v1.Edges.First()));
            //e1=e2
            Assert.IsFalse(e1.Equals(e2));




        }
    }
}
